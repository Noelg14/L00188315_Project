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
        _cacheService = cacheService;
        _configuration = configuration;
        _keyVaultService = keyVaultService;
        _mtlsClient = ConfigureMtlsClient();
        _httpClient = new HttpClient();
        _consentRepository = consentRepository;
        _logger = logger;
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
            var response = await _mtlsClient.PostAsync(url, form);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine($"Error getting access token: {response.StatusCode}");
            }
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<TokenDTO>(content);
            _cacheService.Set("RevolutToken", token!.access_token, token.expires_in);

            return token.access_token;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting access token: {ex.Message}");
            return ex.Message;
        }
    }

    public async Task<Balance> GetAccountBalanceAsync(string accountId, string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return null;
        var token = _cacheService.Get(userId);
        if (string.IsNullOrEmpty(token))
        {
            throw new TokenNullException("No Token for Revolut");
        }
        var response = await sendGetRequestAsync(accountId, "/balances", token);

        var balance = response.Data.Balance.FirstOrDefault(x => x.AccountId == accountId);
        if (balance == null)
        {
            return null;
        }
        var balanceEntity = _mapper.MapToBalanceEntity(balance, accountId);
        balanceEntity.Account = await _accountRepository.GetAccountAsync(userId,accountId);
        await _balanceRepository.CreateBalanceAsync(userId, balanceEntity);
        return balanceEntity;
    }

    public async Task<List<Account>> GetAccountsAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return null;

        var existingAccounts = await _accountRepository.GetAllAccountsAsync(userId);
        if (existingAccounts.Count > 0)
        {
            _logger.LogInformation("Accounts exist - Returning");
            return existingAccounts;
        } // check if accounts already exist

        var token = _cacheService.Get(userId);
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        var response = await sendGetRequestAsync(string.Empty, string.Empty, token);
        var accounts = new List<Account>();
        foreach (var account in response.Data.Account)
        {
            var existingAccount = await _accountRepository.GetAccountAsync(userId, account.AccountId);

            if (existingAccount == null)
            {
                accounts.Add(await _accountRepository.CreateAccountAsync(userId, _mapper.MapToAccountEntity(account, userId)));
            }
        }
        return accounts;
    }

    public async Task<string> GetConsentRequestAsync(string userId)
    {
        var existingConsents = await _consentRepository.GetAllConsentsAsync(userId);
        var consent = existingConsents
            .Where(c => c.Provider == "Revolut" && c.Expires > DateTime.Now) // check if another valid consent exists
            .FirstOrDefault();
        if (consent != null)
        {
            //if user has an existing consent, return the login path
            return await GenerateLoginPath(consent.ConsentId);
        }

        var jso = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var url = _configuration["Revolut:consentUrl"];
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
            var token = await GetAccessToken();
            if (string.IsNullOrEmpty(token))
            {
                return string.Empty;
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

            var response = await _httpClient.SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<OpenBankingDataModel>(jso);
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
                return await GenerateLoginPath(content.Data.ConsentId);
            }
            else
            {
                _logger.LogError($"Error getting consent request: {response.StatusCode}");
                return await response.Content.ReadAsStringAsync();
            }
        }
    }

    public async Task<List<Transaction>> GetTransactionsAsync(string accountId, string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return null;
        var token = _cacheService.Get(userId);
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }
        var existingTransactions = await _transactionRepository.GetAllTransactionsByAccountIdAsync(userId, accountId);
        if(existingTransactions is not null)
        {
            return existingTransactions;
        }
        var data = await sendGetRequestAsync(accountId, "/transactions", token);
        var transactions = new List<Transaction>();
        foreach (var transaction in data.Data.Transaction)
        {
            var entity = _mapper.MapToTransactionEntity(transaction, accountId);
            entity.RootAccountId = accountId;
            transactions.Add(entity);
        }
        await _transactionRepository.CreateTransactionsAsync(transactions);
        return transactions;
    }

    public async Task UpdateConsent(string consentId, ConsentStatus status)
    {
        var consent = await _consentRepository.GetConsentAsync(consentId);
        if (consent == null)
        {
            throw new KeyNotFoundException("Consent not found");
        }
        consent.ConsentStatus = status;
        await _consentRepository.UpdateConsentAsync(consent, status);
        return;
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
                Console.WriteLine($"Error getting access token: {response.StatusCode}");
                return response.StatusCode.ToString();
            }
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<TokenDTO>(content);

            _cacheService.Set($"{userId}", token.access_token, token.expires_in);
            return token.access_token;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting access token: {ex.Message}");
            return ex.Message;
        }
    }

    private async Task<string> GenerateJWT(string consentId)
    {
        var keyPem = File.ReadAllText(_configuration["Revolut:keyPath"]!);
        var clientId = await _keyVaultService.GetSecretAsync("revolutClientId");
        var redirect = _configuration["Revolut:redirectUri"];

        // Load the certificate and key from PEM files
        var rsa = RSA.Create();
        rsa.ImportFromPem(keyPem.ToCharArray());

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
                new RsaSecurityKey(rsa) { KeyId = "68d032ce-b2c3-43dd-b6a5-fb6f095f7b3b" },
                SecurityAlgorithms.RsaSsaPssSha256
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

        var certWithKey = new X509Certificate2(pfxBytes);

        var clientHandler = new HttpClientHandler
        {
            SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13,
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback = (
                httpRequestMessage,
                cert,
                certChain,
                policyErrors
            ) =>{
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
                return content;
            }
            else
            {
                Console.WriteLine($"Error getting transactions: {response.StatusCode}");
                return null;
            }
        }
    }

    public async Task<Consent> GetConsentByIdAsync(string consentId)
    {
        if (string.IsNullOrEmpty(consentId))
            return null;
        return await _consentRepository.GetConsentAsync(consentId);
    }
}
