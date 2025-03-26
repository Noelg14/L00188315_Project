using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L00188315_Project.Core.Entities;

namespace L00188315_Project.Core.Interfaces.Repositories
{
    /// <summary>
    /// Repository for managing transactions
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Get a transaction by its id
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public Task<Transaction> GetTransactionByIdAsync(string transactionId);

        /// <summary>
        /// Get all transactions for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<IReadOnlyList<Transaction>> GetAllTransactionsAsync(string userId);

        /// <summary>
        /// Get all transactions for a user by account id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public Task<IReadOnlyList<Transaction>> GetAllTransactionsByAccountIdAsync(
            string userId,
            string accountId
        );

        /// <summary>
        /// Add transactions into DB
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns></returns>
        public Task CreateTransactionsAsync(List<Transaction> transactions);
    }
}
