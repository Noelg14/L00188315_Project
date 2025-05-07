using L00188315_Project.Core.Entities;
using L00188315_Project.Core.Interfaces.Repositories;
using L00188315_Project.Core.Models;
using L00188315_Project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IReadOnlyList<Transaction>> GetAllTransactionsAsync(string userId)
        {
            return await _dbContext.Transactions
                .Where(t => t.Account.UserId == userId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Transaction>> GetAllTransactionsByAccountIdAsync(string userId, string accountId)
        {
            return await _dbContext.Transactions
                .Where(t => t.Account.UserId == userId && t.AccountId == accountId)
                .ToListAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(string transactionId)
        {
            return await _dbContext.Transactions.FindAsync(transactionId);
        }
    }
}
