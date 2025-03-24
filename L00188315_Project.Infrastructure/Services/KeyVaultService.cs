using Azure;
using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Infrastructure.Services.DTOs;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.ConstrainedExecution;
using System.Text;
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

        var keyVaultBaseUrl = CreateKeyVaultRequestUrl("certificates", certName);
        var cert = await _client.GetFromJsonAsync<KeyVaultCertDTO>(keyVaultBaseUrl);
        if(cert == null)
        {
            throw new Exception("Certificate not found");
        }
        return cert.cer;
    }

    public async Task<string> GetSecretAsync(string secretName)
    {
        var token = await GetToken();
        if (!string.IsNullOrEmpty(_cache.Get(secretName)))
        {
            return _cache.Get(secretName);
        }
        var keyVaultBaseUrl = CreateKeyVaultRequestUrl("secrets", secretName);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var secret = await _client.GetFromJsonAsync<KeyVaultSecretDTO>(keyVaultBaseUrl);
        if (secret == null)
        {
            throw new Exception("secret not found");
        }
        _cache.Set(secretName, secret.value, 3600);

        return secret.value;
    }
    private async Task<string> GetToken()
    {
        var token = _cache.Get("KeyVaultToken"); //  check if we have a token in the cache
        if (!string.IsNullOrEmpty(token))
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
        var value = _cache.Set("KeyVaultToken", tokenRepsonseJson.access_token, tokenRepsonseJson.expires_in);
        return value;
    }
    private string CreateKeyVaultRequestUrl(string secretType,string secretName)
    {
        if(secretType != "secrets" && secretType != "certificates")
        {
            throw new ArgumentException("Invalid secret type");
        }

        var keyVaultBaseUrl = _config["kvSettings:KvBaseUrl"];
        var keyVaultApiVersion = _config["kvSettings:KvApiVersion"];
        var stringBuilder = new StringBuilder();

        stringBuilder.Append(keyVaultBaseUrl);
        stringBuilder.Append($"/{secretType}/{secretName}");
        stringBuilder.Append($"?api-version={keyVaultApiVersion}");

        return stringBuilder.ToString();
    }
}
