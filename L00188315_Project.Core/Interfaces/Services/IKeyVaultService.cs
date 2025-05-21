namespace L00188315_Project.Core.Interfaces.Services;

/// <summary>
/// Key Vault service interface. Allows for changing the Key Vault service implementation without changing the code that uses it.
/// </summary>
public interface IKeyVaultService
{
    /// <summary>
    /// Get a secret from the Key Vault
    /// </summary>
    /// <param name="secretName"></param>
    /// <returns>The secret value</returns>
    public Task<string> GetSecretAsync(string secretName);

    /// <summary>
    /// Get a certificate from the Key Vault
    /// </summary>
    /// <param name="certName"></param>
    /// <returns>The Cert Content</returns>
    public Task<string> GetCertAsync(string certName);
}
