using System.Data;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using L00188315_Project.Core.Entities;
using L00188315_Project.Core.Interfaces.Repositories;
using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Core.Models;
using L00188315_Project.Infrastructure.Exceptions;
using L00188315_Project.Infrastructure.Services.DTOs;
using L00188315_Project.Infrastructure.Services.Mapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace L00188315_Project.Infrastructure.Services;

/// <summary>
/// The implementation of the Revolut service
/// </summary>
public class RevolutService : IRevolutService
{
    private readonly HttpClient _mtlsClient;
    private readonly HttpClient _httpClient;
    private readonly ICacheService _cacheService;
    private readonly IConfiguration _configuration;
    private readonly IKeyVaultService _keyVaultService;
    private readonly IConsentRepository _consentRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IBalanceRepository _balanceRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly OpenBankingMapper _mapper;
    private readonly ILogger<RevolutService> _logger;

    /// <summary>
    /// Revolut Service Constructor
    /// </summary>
    /// <param name="cacheService"></param>
    /// <param name="configuration"></param>
    /// <param name="keyVaultService"></param>
    /// <param name="consentRepository"></param>
    /// <param name="logger"></param>
    /// <param name="accountRepository"></param>
    /// <param name="balanceRepository"></param>
    /// <param name="transactionRepository"></param>
    /// <param name="mapper"></param>
    public RevolutService(
        ICacheService cacheService,
        IConfiguration configuration,
        IKeyVaultService keyVaultService,
        IConsentRepository consentRepository,
        ILogger<RevolutService> logger,
        IAccountRepository accountRepository,
        IBalanceRepository balanceRepository,
        ITransactionRepository transactionRepository,
        OpenBankingMapper mapper
    )
    {
        _logger = logger;
        _cacheService = cacheService;
        _configuration = configuration;
        _keyVaultService = keyVaultService;
        _mtlsClient = ConfigureMtlsClient();
        _httpClient = new HttpClient();
        _consentRepository = consentRepository;
        _accountRepository = accountRepository;
        _mapper = mapper;
        _balanceRepository = balanceRepository;
        _transactionRepository = transactionRepository;
    }

    /// <summary>
    /// Gets the access token for the Revolut API - For generating consents
    /// </summary>
    /// <returns></returns>
    private async Task<string> GetAccessToken()
    {
        var accessToken = _cacheService.Get("RevolutToken");
        if (!string.IsNullOrEmpty(accessToken))
        {
            return accessToken;
        }
        try
        {
            var url = _configuration["Revolut:tokenUrl"];

            var clientId = await _keyVaultService.GetSecretAsync("revolutClientId");

            var kvp = new List<KeyValuePair<string, string>>
            {
                KeyValuePair.Create("client_id", clientId),
                KeyValuePair.Create("scope", "accounts"),
                KeyValuePair.Create("grant_type", "client_credentials"),
            };

            var form = new FormUrlEncodedContent(kvp);
            _logger.LogInformation("Sending request to Revolut API");

            var response = await _mtlsClient.PostAsync(url, form);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError(
                    "Error getting access token: {0} \n {1}",
                    response.StatusCode,
                    response.Content.ToString()
                );
            }
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<TokenDTO>(content);
            _cacheService.Set("RevolutToken", token!.access_token, token.expires_in);
            _logger.LogInformation("Got token for to generate consent");
            return token.access_token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting access token: {0}", ex.Message);
            throw new TokenException(ex.Message);
        }
    }

    /// <summary>
    /// Gets the account balance for the provided accountId - from DB if exists
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="TokenNullException"></exception>
    /// <exception cref="BalanceException"></exception>
    public async Task<Balance> GetAccountBalanceAsync(string accountId, string userId)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentNullException(nameof(userId), FailureReason.USER_ID_NULL);
        var existingBalance = await _balanceRepository.GetBalanceAsync(userId, accountId);
        if (existingBalance is not null)
        {
            _logger.LogInformation("Balance exists - Returning");
            return existingBalance;
        } // check if balance already exists

        var token = _cacheService.Get(userId);
        if (string.IsNullOrEmpty(token))
        {
            throw new TokenNullException("No Token for Revolut");
        }
        var response = await sendGetRequestAsync(accountId, "/balances", token);
        if (response!.Data!.Balance == null)
        {
            throw new BalanceException("No Balance returned after call to Revolut");
        }
        var balance = response!.Data!.Balance!.FirstOrDefault(x => x.AccountId == accountId);
        if (balance!.CreditDebitIndicator!.ToLower() == "debit")
        {
            balance.Amount!._Amount = "-" + balance.Amount._Amount!; // add '-' to the start,are the user is in debit.
        }

        var balanceEntity = _mapper.MapToBalanceEntity(balance, accountId);
        balanceEntity.Account = await _accountRepository.GetAccountAsync(userId, accountId);
        await _balanceRepository.CreateBalanceAsync(userId, balanceEntity);
        return balanceEntity;
    }

    /// <summary>
    ///  Gets the accounts for the Revolut API - from DB if exists
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="TokenNullException"></exception>
    public async Task<List<Account>> GetAccountsAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentNullException(nameof(userId), FailureReason.USER_ID_NULL);

        var existingAccounts = await _accountRepository.GetAllAccountsAsync(userId);
        if (existingAccounts is not null && existingAccounts.Count > 0)
        {
            _logger.LogInformation("Accounts exist - Returning");
            return existingAccounts;
        } // check if accounts already exist

        var token = _cacheService.Get(userId);
        if (string.IsNullOrEmpty(token))
        {
            throw new TokenNullException("No Token for Revolut - Refresh Link");
        }

        var response = await sendGetRequestAsync(string.Empty, string.Empty, token);
        var accounts = new List<Account>();
        foreach (var account in response.Data!.Account!)
        {
            var existingAccount = await _accountRepository.GetAccountAsync(
                userId,
                account.AccountId!
            );

            if (existingAccount == null)
            {
                accounts.Add(
                    await _accountRepository.CreateAccountAsync(
                        userId,
                        _mapper.MapToAccountEntity(account, userId)
                    )
                );
            }
        }
        return accounts;
    }

    /// <summary>
    /// Gets the consent request for the Revolut API - Generates a new consent if one does not exist
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="TokenNullException"></exception>
    /// <exception cref="ConsentException"></exception>
    public async Task<string> GetConsentRequestAsync(string userId)
    {
        var existingConsents = await _consentRepository.GetAllConsentsAsync(userId);
        var consent = existingConsents.FirstOrDefault(c =>
            c.Provider == "Revolut" && c.Expires > DateTime.Now
        );
        if (consent != null)
        {
            //if user has an existing consent, return the login path
            return await GenerateLoginPath(consent.ConsentId);
        }

        var jso = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var url = _configuration["Revolut:consentUrl"];
        _logger.LogInformation("Retrieved consent URL: {0}", url);

        var requestData = new Core.Models.Data
        {
            Permissions = new List<string>
            {
                "ReadAccountsBasic",
                "ReadAccountsDetail",
                "ReadTransactionsDebits",
                "ReadTransactionsDetail",
                "ReadBalances",
            },
            ExpirationDateTime = DateTime.Now.AddDays(1),
            TransactionFromDateTime = DateTime.Now.AddYears(-2),
            TransactionToDateTime = DateTime.Now,
        };
        var consentRequest = new OpenBankingDataModel { Data = requestData };
        using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url))
        {
            try
            {
                var token = await GetAccessToken();
                if (string.IsNullOrEmpty(token))
                {
                    throw new TokenNullException("Token is null");
                }
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
                httpRequestMessage.Headers.Add("x-fapi-financial-id", "001580000103UAvAAM");
                httpRequestMessage.Content = new StringContent(
                    JsonSerializer.Serialize(consentRequest),
                    Encoding.UTF8,
                    "application/json"
                );
                _logger.LogInformation("Sending consent request to Revolut API");

                var response = await _httpClient.SendAsync(httpRequestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<OpenBankingDataModel>(
                        jso
                    );
                    await _consentRepository.CreateConsentAsync(
                        new Consent
                        {
                            ConsentId = content?.Data?.ConsentId!,
                            UserId = userId,
                            ConsentStatus = ConsentStatus.Pending,
                            Provider = "Revolut",
                            Scopes = string.Join(", ", requestData.Permissions),
                            Expires = DateTime.Now.AddDays(1),
                        }
                    );
                    return await GenerateLoginPath(content?.Data?.ConsentId!);
                }
                else
                {
                    _logger.LogError(
                        "Error getting consent request {0} : {1}",
                        response.StatusCode,
                        response.Content.ToString()
                    );
                    throw new ConsentException(
                        response.Content.ToString() ?? "An Error occurred when getting a consent"
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting consent request:{0}", ex.Message);
                throw new ConsentException(ex.Message);
            }
        }
    }

    /// <summary>
    /// Gets the transactions for the Revolut API - from DB if exists
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="TokenNullException"></exception>
    public async Task<List<Transaction>> GetTransactionsAsync(string accountId, string userId)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentNullException(nameof(userId), FailureReason.USER_ID_NULL);

        var existingTransactions = await _transactionRepository.GetAllTransactionsByAccountIdAsync(
            userId,
            accountId
        );
        if (existingTransactions is not null && existingTransactions.Count > 0)
        {
            _logger.LogInformation("Transactions exist - Returning");
            return existingTransactions!; // We know that the transactions exist
        }

        var token = _cacheService.Get(userId);
        if (string.IsNullOrEmpty(token))
        {
            throw new TokenNullException("Please Refresh Token");
        }
        var data = await sendGetRequestAsync(accountId, "/transactions", token);
        var transactions = new List<Transaction>();
        foreach (var transaction in data?.Data?.Transaction!)
        {
            var transactionExists = await _transactionRepository.GetTransactionByIdAsync(transaction!.TransactionId!);
            if (transactionExists is not null && transaction.AccountId != transactionExists!.Account!.AccountId) // dont load same transaction twice
                continue;
            var entity = _mapper.MapToTransactionEntity(transaction, accountId);
            entity.Account = await _accountRepository.GetAccountAsync(userId, accountId);
            transactions.Add(entity);
        }
        await _transactionRepository.CreateTransactionsAsync(transactions);
        return transactions;
    }

    /// <summary>
    /// Updates a consent status in the database
    /// </summary>
    /// <param name="consentId"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task UpdateConsent(string consentId, ConsentStatus status)
    {
        var consent = await _consentRepository.GetConsentAsync(consentId);
        if (consent == null)
        {
            throw new KeyNotFoundException("Consent not found");
        }
        consent.ConsentStatus = status;
        await _consentRepository.UpdateConsentAsync(consent, status);
    }

    /// <summary>
    /// Gets the users access token, after consent is received
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task<string> GetUserAccessToken(string userId, string code)
    {
        try
        {
            var url = _configuration["Revolut:tokenUrl"];
            var kvp = new List<KeyValuePair<string, string>>
            {
                KeyValuePair.Create("code", code),
                KeyValuePair.Create("grant_type", "authorization_code"),
            };

            var form = new FormUrlEncodedContent(kvp);
            var response = await _mtlsClient.PostAsync(url, form);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError("Error getting access token: {0}", response.StatusCode);
                return response.StatusCode.ToString();
            }
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<TokenDTO>(content);

            _cacheService.Set($"{userId}", token!.access_token, token.expires_in);
            return token.access_token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting access token: {0}", ex.Message);
            return ex.Message;
        }
    }

    /// <summary>
    /// Generates a signed JWT token for the Revolut API
    /// </summary>
    /// <param name="consentId"></param>
    /// <returns></returns>
    private async Task<string> GenerateJWT(string consentId)
    {
        var keyPem = await File.ReadAllTextAsync(_configuration["Revolut:keyPath"]!);
        var clientId = await _keyVaultService.GetSecretAsync("revolutClientId");
        var redirect = _configuration["Revolut:redirectUri"];

        // Load the certificate and key from PEM files
        var rsa = RSA.Create();
        rsa.ImportFromPem(keyPem.ToCharArray());

        _logger.LogInformation("Loaded PEM from Path, Size: {0}", keyPem.Length);
        _logger.LogInformation("Loaded RSA from PEM, Key Size: {0}", rsa.KeySize);

        var claimsDictionary = new Dictionary<string, object>
        {
            {
                "id_token",
                new Dictionary<string, object>
                {
                    {
                        "openbanking_intent_id",
                        new Dictionary<string, object> { { "value", consentId } }
                    },
                }
            },
        };
        var header = new JwtHeader(
            new SigningCredentials(
                new RsaSecurityKey(rsa) { KeyId = "68d032ce-b2c3-43dd-b6a5-fb6f095f7b3b" }, // kid of our JWKS
                SecurityAlgorithms.RsaSsaPssSha256 // as requested by Revolut
            )
        );
        var payload = new JwtPayload
        {
            { "response_type", "code id_token" },
            { "client_id", clientId },
            { "redirect_uri", redirect },
            { "scope", "accounts" },
            { "claims", claimsDictionary },
        };
        var tokenString = new JwtSecurityToken(header, payload);
        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(tokenString);
    }

    /// <summary>
    /// Configures the mTLS client used for authenticating with the Revolut API
    /// </summary>
    /// <returns></returns>
    private HttpClient ConfigureMtlsClient()
    {
        var pfxBytes = File.ReadAllBytes(_configuration["Revolut:pfxPath"]!);
        _logger.LogInformation("PFX Size : {0}", pfxBytes.Length);

        var certWithKey = new X509Certificate2(
            pfxBytes,
            string.Empty,
            X509KeyStorageFlags.MachineKeySet
                | X509KeyStorageFlags.Exportable
                | X509KeyStorageFlags.PersistKeySet
        );
        if (Debugger.IsAttached) // Required for this to work locally??????
        {
            certWithKey = new X509Certificate2(pfxBytes);
        }
        _logger.LogInformation("Cert has private key: {0}", certWithKey.HasPrivateKey);

        var clientHandler = new HttpClientHandler
        {
            SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13,
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback = (
                httpRequestMessage,
                cert,
                certChain,
                policyErrors
            ) =>
            {
                return true; // allow insecure / self signed certificates / don't validate certs
            },
            Credentials = null,
            CheckCertificateRevocationList = false,
        };
        clientHandler.ClientCertificates.Add(certWithKey);
        return new HttpClient(clientHandler);
    }

    /// <summary>
    /// Helper method to generate the login path for the customer SCA
    /// </summary>
    /// <param name="consentId"></param>
    /// <returns></returns>
    private async Task<string> GenerateLoginPath(string consentId)
    {
        var jwt = await GenerateJWT(consentId);

        var loginPath = _configuration["Revolut:loginUrl"];
        var clientId = await _keyVaultService.GetSecretAsync("revolutClientId");

        loginPath += $"&redirect_uri={_configuration["Revolut:redirectUri"]}";
        loginPath += $"&client_id={clientId}";
        loginPath += $"&request={jwt}";
        return loginPath;
    }

    /// <summary>
    /// Generic method to send requests to the Revolut API
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="endpoint"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="DataException"></exception>
    private async Task<OpenBankingDataModel> sendGetRequestAsync(
        string? accountId,
        string? endpoint,
        string token
    )
    {
        var url = _configuration["Revolut:baseUrl"] + "/accounts";
        if (!string.IsNullOrEmpty(accountId))
            url += $"/{accountId}";
        if (!string.IsNullOrEmpty(accountId))
            url += endpoint;

        using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url))
        {
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );
            httpRequestMessage.Headers.Add("x-fapi-financial-id", "001580000103UAvAAM");

            var response = await _httpClient.SendAsync(httpRequestMessage);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<OpenBankingDataModel>();
                return content!;
            }
            else
            {
                _logger.LogError(
                    "Error getting data: {0} | {1}",
                    response.StatusCode,
                    response.Content.ToString()
                );
                throw new DataException($"Error Getting data from {endpoint}");
            }
        }
    }

    /// <summary>
    /// Gets the consent for the provided consentId from DB
    /// </summary>
    /// <param name="consentId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<Consent> GetConsentByIdAsync(string consentId)
    {
        if (string.IsNullOrEmpty(consentId))
            throw new ArgumentNullException(nameof(consentId), FailureReason.CONSENT_ID_NULL);
        return await _consentRepository.GetConsentAsync(consentId);
    }

    /// <summary>
    ///  Get an account by its id from DB
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<Account> GetAccountAsync(string accountId, string userId)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentNullException(nameof(userId), FailureReason.USER_ID_NULL);
        if (string.IsNullOrEmpty(accountId))
            throw new ArgumentNullException(nameof(accountId), FailureReason.ACCOUNT_ID_NULL);
        return await _accountRepository.GetAccountAsync(userId, accountId);
    }

    /// <summary>
    ///  Removes an account from the DB by its TRUE id - not the accountId
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="AccountException"></exception>
    public async Task DeleteAccountAsync(string accountId)
    {
        if (string.IsNullOrEmpty(accountId))
            throw new ArgumentNullException(nameof(accountId), FailureReason.ACCOUNT_ID_NULL);
        var complete = await _accountRepository.DeleteAccountAsync(accountId);
        if (!complete)
        {
            throw new AccountException("Error deleting account");
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<List<Transaction>> GetTransactionsForUserAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentNullException(nameof(userId), FailureReason.USER_ID_NULL);
        var accounts = await _accountRepository.GetAllAccountsAsync(userId);
        var transactions = new List<Transaction>();
        if (accounts is null || accounts.Count == 0)
        {
            return transactions;
        }
        foreach (var account in accounts)
        {
            var accountTransactions =
                await _transactionRepository.GetAllTransactionsByAccountIdAsync(
                    userId,
                    account.AccountId
                );
            transactions.AddRange(accountTransactions);
        }
        return transactions;
    }
}
