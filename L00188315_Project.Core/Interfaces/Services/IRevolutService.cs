﻿using L00188315_Project.Core.Entities;

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
    /// Gets the account for the provided accountId
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="userId"></param>
    /// <returns>An Account related to the AccountId provided</returns>
    public Task<Account> GetAccountAsync(string accountId, string userId);

    /// <summary>
    /// Deletes the provided account.
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public Task DeleteAccountAsync(string accountId);

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

    /// <summary>
    /// Gets all transactions for a user and their accounts
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<List<Transaction>> GetTransactionsForUserAsync(string userId);
}
