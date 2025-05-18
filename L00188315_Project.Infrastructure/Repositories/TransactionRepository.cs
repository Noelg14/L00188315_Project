using L00188315_Project.Core.Entities;
using L00188315_Project.Core.Interfaces.Repositories;
using L00188315_Project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace L00188315_Project.Infrastructure.Repositories
{
    public class TransactionRepository(AppDbContext _dbContext) : ITransactionRepository
    {
        public async Task CreateTransactionsAsync(List<Transaction> transactions)
        {
            await _dbContext.Transactions.AddRangeAsync(transactions);
            await _dbContext.SaveChangesAsync();
            return;
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync(string userId)
        {
            return await _dbContext
                .Transactions.Where(t => t.Account!.UserId == userId)
                .ToListAsync();
        }

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

        public async Task<Transaction> GetTransactionByIdAsync(string transactionId)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _dbContext.Transactions.FindAsync(transactionId);
#pragma warning restore CS8603 // Possible null reference return.
            // it's okay to return null here, as we are checking for null in the service.
        }
    }
}
