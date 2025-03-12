using L00188315_Project.Core.Interfaces.Services;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using L00188315_Project.Infrastructure.Services.DTOs;
using L00188315_Project.Core.Entities;

namespace L00188315_Project.Infrastructure.Services;
public class RevolutService(ICacheService _cacheService,
    IConfiguration _configuration
    ) : IRevolutService
{
    public async Task GetAccessToken()
    {
        if(await _cacheService.Get<string>("RevolutToken") != null)
        {
            return; // we already have a token
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

            var consentId = await GetConsentAsync();
            //return consentId;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting access token: {ex.Message}");
            //return ex.Message;
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

    public Task<string> GetConsentAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<Transaction>> GetTransactionsAsync(string accountId)
    {
        throw new NotImplementedException();
    }
}

