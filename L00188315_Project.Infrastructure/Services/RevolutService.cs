using L00188315_Project.Core.Interfaces.Services;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using L00188315_Project.Infrastructure.Services.DTOs;
using System.Net.Http.Headers;
using System.Text;
using L00188315_Project.Core.Models;
using Transaction = L00188315_Project.Core.Models.Transaction;
using Account = L00188315_Project.Core.Models.Account;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace L00188315_Project.Infrastructure.Services;
public class RevolutService : IRevolutService
{
    private HttpClient _mtlsClient;
    private HttpClient _httpClient;
    private readonly ICacheService _cacheService;
    private readonly IConfiguration _configuration;
    private readonly IKeyVaultService _keyVaultService;

    public RevolutService(ICacheService cacheService,
        IConfiguration configuration,
        IKeyVaultService keyVaultService
    )
    {
        _cacheService = cacheService;
        _configuration = configuration;
        _keyVaultService = keyVaultService;
        _mtlsClient = ConfigureMtlsClient();
        _httpClient = new HttpClient();
    }

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

            var kvp = new List<KeyValuePair<string, string>>{
                    KeyValuePair.Create("client_id", clientId),
                    KeyValuePair.Create("scope", "accounts"),
                    KeyValuePair.Create("grant_type", "client_credentials")
            };

            var form = new FormUrlEncodedContent(kvp);
            var response = await _mtlsClient.PostAsync(url, form);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine($"Error getting access token: {response.StatusCode}");
                //return response.StatusCode.ToString();
            }
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<TokenDTO>(content);
            _cacheService.Set("RevolutToken", token.access_token,token.expires_in);

            return token.access_token;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting access token: {ex.Message}");
            return ex.Message;
        }
    }

    public Task<string> GetAccountBalanceAsync(string accountId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Account>> GetAccountsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetConsentAsync(string userId)
    {
     
        var jso = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var url = _configuration["Revolut:consentUrl"];
        var requestData = new Core.Models.Data
        {
            Permissions = new List<string> { "ReadAccountsBasic", "ReadAccountsDetail", "ReadTransactionsDebits", "ReadTransactionsDetail", "ReadBalances" },
            ExpirationDateTime = DateTime.Now.AddDays(1),
            TransactionFromDateTime = DateTime.Now.AddYears(-2),
            TransactionToDateTime = DateTime.Now
        };
        var consentRequest = new OpenBankingDataModel
        {
            Data = requestData
        };
        using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url))
        {
            var token = await GetAccessToken();
            if (string.IsNullOrEmpty(token))
            {
                return string.Empty;
            }
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            httpRequestMessage.Headers.Add("x-fapi-financial-id", "001580000103UAvAAM");
            httpRequestMessage.Content = new StringContent(JsonSerializer.Serialize(consentRequest), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequestMessage); 

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<OpenBankingDataModel>(jso);
                var jwt = GenerateJWT(content.Data.ConsentId);

                var loginPath = _configuration["Revolut:loginUrl"];
                var clientId = await _keyVaultService.GetSecretAsync("revolutClientId");


                loginPath += $"&redirect_uri={_configuration["Revolut:redirectUri"]}";
                loginPath += $"&client_id={clientId}";
                loginPath += $"&request={jwt}";

                return loginPath;


                //return await response.Content.ReadAsStringAsync();
            }
            else
            {
                Console.WriteLine($"Error getting consent request: {response.StatusCode}");
                return await response.Content.ReadAsStringAsync();
            }
        }
    }

    public Task<List<Transaction>> GetTransactionsAsync(string accountId)
    {
        throw new NotImplementedException();
    }
    private string GenerateJWT(string consentId)
    {          
        var keyPem = File.ReadAllText(_configuration["Revolut:keyPath"]);
        var clientId = _keyVaultService.GetSecretAsync("revolutClientId").Result;
        var redirect = _configuration["Revolut:redirectUri"];

        // Load the certificate and key from PEM files
        var rsa = RSA.Create();
        rsa.ImportFromPem(keyPem.ToCharArray());


        var header = new JwtHeader(new SigningCredentials(new RsaSecurityKey(rsa) { KeyId = "68d032ce-b2c3-43dd-b6a5-fb6f095f7b3b" }, SecurityAlgorithms.RsaSsaPssSha256));
        var payload = new JwtPayload
            {
                { "response_type", "code id_token" },
                { "client_id", clientId },
                { "redirect_uri", redirect },
                { "scope", "accounts" },
                { "claims","\"id_token\":{\"openbanking_intent_id\":{\"value\":"+consentId+"}"}, // tried anonymous types, but it didnt work :shrug:
            };
        var tokenString = new JwtSecurityToken(header, payload);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenString);

    }
    private HttpClient ConfigureMtlsClient()
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        ServicePointManager.CheckCertificateRevocationList = false;

        var pfxBytes = File.ReadAllBytes(_configuration["Revolut:pfxPath"]);

        var certWithKey = new X509Certificate2(pfxBytes);

        var clientHandler = new HttpClientHandler
        {
            SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13,
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, certChain, policyErrors) =>
            {
                return true; // allow insecure / self signed certificates
            },
            Credentials = null,
        };

        clientHandler.ClientCertificates.Add(certWithKey);
        return new HttpClient(clientHandler);
    }
}

