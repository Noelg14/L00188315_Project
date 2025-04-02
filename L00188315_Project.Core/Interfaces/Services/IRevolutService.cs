using L00188315_Project.Core.Entities;


namespace L00188315_Project.Core.Interfaces.Services;

public interface IRevolutService
{
    /// <summary>
    /// Get the consent for the Revolut API
    /// </summary>
    /// <returns></returns>
    public Task<string> GetConsentRequestAsync(string userId);

    /// <summary>
    /// Get the accounts for the Revolut API
    /// </summary>
    /// <returns></returns>
    public Task<List<Account>> GetAccountsAsync(string userId);

    /// <summary>
    /// Get the transactions for the Revolut API
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public Task<List<Transaction>> GetTransactionsAsync(string accountId, string userId);

    /// <summary>
    ///    Get the balance for the Revolut API
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public Task<Balance> GetAccountBalanceAsync(string accountId, string userId);

    /// <summary>
    /// Update the consent status
    /// </summary>
    /// <param name="consentId"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public Task UpdateConsent(string consentId, ConsentStatus status);

    /// <summary>
    /// Gets the users access token, after consent is received
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="code"></param>
    /// <returns>The access token</returns>
    public Task<string> GetUserAccessToken(string userId, string code);
    /// <summary>
    /// Get the consent by Id
    /// </summary>
    /// <param name="consentId"></param>
    /// <returns></returns>
    public Task<Consent> GetConsentByIdAsync(string consentId);
}
