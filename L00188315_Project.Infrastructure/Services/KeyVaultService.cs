using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Infrastructure.Exceptions;
using L00188315_Project.Infrastructure.Services.DTOs;
using Microsoft.Extensions.Configuration;

namespace L00188315_Project.Infrastructure.Services;

/// <summary>
/// // Implementation of the KeyVaultService. This service is used to get secrets and certificates from Azure Key Vault.
/// </summary>
/// <param name="_httpClientFactory"></param>
/// <param name="_config"></param>
/// <param name="_cache"></param>
public class KeyVaultService(
    IHttpClientFactory _httpClientFactory,
    IConfiguration _config,
    ICacheService _cache
) : IKeyVaultService
{
    private HttpClient _client = _httpClientFactory.CreateClient("KeyVaultClient");

    /// <summary>
    /// Gets a certificate from Azure Key Vault.
    /// </summary>
    /// <param name="certName"></param>
    /// <returns></returns>
    /// <exception cref="KeyVaultException"></exception>
    public async Task<string> GetCertAsync(string certName)
    {
        var token = await GetToken();

        var keyVaultBaseUrl = CreateKeyVaultRequestUrl("certificates", certName);
        try
        {
            var cert = await _client.GetFromJsonAsync<KeyVaultCertDTO>(keyVaultBaseUrl);
            if (cert == null)
            {
                throw new KeyVaultException("Certificate not found");
            }
            return cert.cer!;
        }
        catch (Exception)
        {
            throw new KeyVaultException($"Cert not found");
        }
    }

    /// <summary>
    /// Gets a secret from Azure Key Vault.
    /// </summary>
    /// <param name="secretName"></param>
    /// <returns></returns>
    /// <exception cref="KeyVaultException"></exception>
    public async Task<string> GetSecretAsync(string secretName)
    {
        var token = await GetToken();
        if (!string.IsNullOrEmpty(_cache.Get(secretName)))
        {
            return _cache.Get(secretName);
        }
        var keyVaultBaseUrl = CreateKeyVaultRequestUrl("secrets", secretName);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            token
        );
        try
        {
            var secret = await _client.GetFromJsonAsync<KeyVaultSecretDTO>(keyVaultBaseUrl);
            if (secret == null)
            {
                throw new KeyVaultException("Secret not found");
            }
            _cache.Set(secretName, secret.value, 3600);

            return secret.value;
        }
        catch (Exception ex)
        {
            throw new KeyVaultException($"Error getting Secret: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets a token from Azure Key Vault. This token is used to authenticate with Azure Key Vault.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>
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

        if (
            string.IsNullOrEmpty(clientId)
            || string.IsNullOrEmpty(clientSecret)
            || string.IsNullOrEmpty(tokenUrl)
            || string.IsNullOrEmpty(scope)
        )
        {
            throw new ArgumentNullException(
                "ClientId, ClientSecret, TokenUrl or Scope cannot be null or empty."
            );
        }

        var form = new FormUrlEncodedContent(
            new List<KeyValuePair<string, string>>
            {
                KeyValuePair.Create("client_id", clientId!),
                KeyValuePair.Create("client_secret", clientSecret!),
                KeyValuePair.Create("grant_type", "client_credentials"),
                KeyValuePair.Create("scope", scope!),
            }
        );
        var tokenRepsponse = await _client.PostAsync(tokenUrl, form);
        if (!tokenRepsponse.IsSuccessStatusCode)
        {
            throw new Exception($"Error getting token: {tokenRepsponse.StatusCode}");
        }
        var content = await tokenRepsponse.Content.ReadAsStringAsync();
        var tokenRepsonseJson = JsonSerializer.Deserialize<TokenDTO>(content);
        var value = _cache.Set(
            "KeyVaultToken",
            tokenRepsonseJson!.access_token,
            tokenRepsonseJson.expires_in
        );
        return value;
    }

    /// <summary>
    /// Helper method to create the Key Vault request URL.
    /// </summary>
    /// <param name="secretType"></param>
    /// <param name="secretName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private string CreateKeyVaultRequestUrl(string secretType, string secretName)
    {
        if (secretType != "secrets" && secretType != "certificates")
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
