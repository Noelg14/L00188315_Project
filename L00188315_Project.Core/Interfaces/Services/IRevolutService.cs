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
}
