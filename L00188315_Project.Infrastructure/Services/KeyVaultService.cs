using Azure;
using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Infrastructure.Services.DTOs;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace L00188315_Project.Infrastructure.Services;

public class KeyVaultService(
    IConfiguration _config,
    ICacheService _cache) : IKeyVaultService
{
    private HttpClient _client = new HttpClient();
    public async Task<string> GetCertAsync(string certName)
    {
        var token = await GetToken();
        return certName;
    }

    public async Task<string> GetSecretAsync(string secretName)
    {
        var token = await GetToken();
        var keyVaultUrl = _config["kvSettings:KvUrl"];
        keyVaultUrl = keyVaultUrl.Replace("{secretName}",secretName);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var secret = await _client.GetFromJsonAsync<KeyVaultDTO>(keyVaultUrl);


        return secret.value;
    }
    private async Task<string> GetToken()
    {
        var token = await _cache.Get<string>("KeyVaultToken"); //  check if we have a token in the cache
        if (token != null)
        {
            return token;
        }
        // get token for keyvault
        var clientId = _config["kvSettings:ClientId"];
        var clientSecret = _config["kvSettings:ClientSecret"];
        var tokenUrl = _config["kvSettings:TokenUrl"];
        var scope = _config["kvSettings:Scope"];


        var form = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>{
            KeyValuePair.Create("client_id", clientId),
            KeyValuePair.Create("client_secret", clientSecret),
            KeyValuePair.Create("grant_type", "client_credentials"),
            KeyValuePair.Create("scope", scope)
        });
        var tokenRepsponse = await _client.PostAsync(tokenUrl, form);
        if(!tokenRepsponse.IsSuccessStatusCode)
        {
            throw new Exception($"Error getting token: {tokenRepsponse.StatusCode}");
        }
        var content = await tokenRepsponse.Content.ReadAsStringAsync();
        var tokenRepsonseJson = JsonSerializer.Deserialize<TokenDTO>(content);
        _cache.Set("KeyVaultToken", tokenRepsonseJson.access_token, tokenRepsonseJson.expires_in);
        return tokenRepsonseJson.access_token;
    }
}
