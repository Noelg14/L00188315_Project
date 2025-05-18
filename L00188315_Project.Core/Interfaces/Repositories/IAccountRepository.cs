using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L00188315_Project.Core.Entities;

namespace L00188315_Project.Core.Interfaces.Repositories
{
    /// <summary>
    /// Repository for managing accounts
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Get all accounts for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<List<Account>> GetAllAccountsAsync(string userId);

        /// <summary>
        /// Get an account by its id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public Task<Account> GetAccountAsync(string userId, string accountId);

        /// <summary>
        /// Create an account
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public Task<Account> CreateAccountAsync(string userId, Account account);

        /// <summary>
        /// Update an account
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public Task<Account> UpdateAccountAsync(string userId, Account account);
        /// <summary>
        /// Deletes the provided Account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<bool> DeleteAccountAsync(string accountId);
    }
}
