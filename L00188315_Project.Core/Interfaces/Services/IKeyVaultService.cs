
namespace L00188315_Project.Core.Interfaces.Services;

public interface IKeyVaultService
{
    public Task<string> GetSecretAsync(string secretName);
    public Task<string> GetCertAsync(string certName);
}
