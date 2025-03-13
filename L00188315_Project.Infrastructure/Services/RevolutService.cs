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

namespace L00188315_Project.Infrastructure.Services;
public class RevolutService(ICacheService _cacheService,
    IConfiguration _configuration
    ) : IRevolutService
{
    public async Task<string> GetAccessToken()
    {
        if(await _cacheService.Get<string>("RevolutToken") != null)
        {
            return ""; // we already have a token
        }
        try
        {
            var url = _configuration["Revolut:tokenUrl"];
            var clientId = _configuration["Revolut:clientId"];

            var kvp = new List<KeyValuePair<string, string>>{
                    KeyValuePair.Create("client_id", clientId),
                    KeyValuePair.Create("scope", "accounts"),
                    KeyValuePair.Create("grant_type", "client_credentials")
            };

            var form = new FormUrlEncodedContent(kvp);
            var response = await new HttpClient().PostAsync(url, form);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine($"Error getting access token: {response.StatusCode}");
                //return response.StatusCode.ToString();
            }
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<TokenDTO>(content);
            _cacheService.Set("RevolutToken", token.access_token,token.expires_in);

            return "";
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
            var token = await _cacheService.Get<string>("RevolutToken");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            httpRequestMessage.Headers.Add("x-fapi-financial-id", "001580000103UAvAAM");
            httpRequestMessage.Content = new StringContent(JsonSerializer.Serialize(consentRequest), Encoding.UTF8, "application/json");

            var response = await new HttpClient().SendAsync(httpRequestMessage);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<OpenBankingDataModel>(jso);
                var jwt = GenerateJWT(content.Data.ConsentId);
                var loginPath = _configuration["Revolut:loginUrl"];
                loginPath += $"&redirect_uri={_configuration["Revolut:redirectUri"]}";
                loginPath += $"&client_id={_configuration["Revolut:clientId"]}";
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
        var keyByte = File.ReadAllBytes(_configuration["Revolut:keyPath"]);
        // Load the certificate and key from PEM files
        var rsa = RSA.Create();
        rsa.ImportFromPem(keyPem.ToCharArray());
        var redirect = _configuration["Revolut:redirectUri"];

        var header = new JwtHeader(new SigningCredentials(new RsaSecurityKey(rsa) { KeyId = "68d032ce-b2c3-43dd-b6a5-fb6f095f7b3b" }, SecurityAlgorithms.RsaSsaPssSha256));
        var payload = new JwtPayload
            {
                { "response_type", "code id_token" },
                { "client_id", _configuration["Revolut:clientId"] },
                { "redirect_uri", redirect },
                { "scope", "accounts" },
                { "claims", new { id_token = new { openbanking_intent_id = new { value = consentId } } } }
            };
        var tokenString = new JwtSecurityToken(header, payload);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenString);

    }
}

