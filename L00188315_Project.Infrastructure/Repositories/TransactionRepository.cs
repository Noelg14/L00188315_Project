using L00188315_Project.Core.Entities;
using L00188315_Project.Core.Interfaces.Repositories;
using L00188315_Project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace L00188315_Project.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of the ITransactionRepository interface for managing transactions.
    /// </summary>
    /// <param name="_dbContext"></param>
    public class TransactionRepository(AppDbContext _dbContext) : ITransactionRepository
    {
        /// <summary>
        /// Create transactions for a user.
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns></returns>
        public async Task CreateTransactionsAsync(List<Transaction> transactions)
        {
            await _dbContext.Transactions.AddRangeAsync(transactions);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Get all transactions for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Transaction>> GetAllTransactionsAsync(string userId)
        {
            return await _dbContext
                .Transactions.Where(t => t.Account!.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Gets All transactions for a user by account id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<List<Transaction>> GetAllTransactionsByAccountIdAsync(
            string userId,
            string accountId
        )
        {
            return await _dbContext
                .Transactions.Where(t =>
                    t.Account!.UserId == userId && t.Account.AccountId == accountId
                )
                .ToListAsync();
        }

        /// <summary>
        /// Get a transaction by its id.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public async Task<Transaction> GetTransactionByIdAsync(string transactionId)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _dbContext.Transactions.FindAsync(transactionId);
#pragma warning restore CS8603 // Possible null reference return.
            // it's okay to return null here, as we are checking for null in the service.
        }
    }
}
