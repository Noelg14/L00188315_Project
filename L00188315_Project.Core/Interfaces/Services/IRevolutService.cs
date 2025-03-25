using Transaction = L00188315_Project.Core.Models.Transaction;
using Account = L00188315_Project.Core.Models.Account;
using L00188315_Project.Core.Entities;

namespace L00188315_Project.Core.Interfaces.Services;
public interface IRevolutService
{
    /// <summary>
    /// Get the consent for the Revolut API
    /// </summary>
    /// <returns></returns>
    public Task<string> GetConsentAsync(string userId);
    /// <summary>
    /// Get the accounts for the Revolut API
    /// </summary>
    /// <returns></returns>
    public Task<List<Account>> GetAccountsAsync();
    /// <summary>
    /// Get the transactions for the Revolut API
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public Task<List<Transaction>> GetTransactionsAsync(string accountId);
    /// <summary>
    ///    Get the balance for the Revolut API
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public Task<string> GetAccountBalanceAsync(string accountId);
    /// <summary>
    /// Update the consent status
    /// </summary>
    /// <param name="consentId"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public Task UpdateConsent(string consentId,ConsentStatus status);
    /// <summary>
    /// Gets the users access token, after consent is received
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="code"></param>
    /// <returns>The access token</returns>
    public Task<string> GetUserAccessToken(string userId,string code);

}
