using L00188315_Project.Core.Entities;
using L00188315_Project.Core.Interfaces.Repositories;
using L00188315_Project.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L00188315_Project.Infrastructure.Repositories
{
    public class TransactionRepository(AppDbContext _dbContext) : ITransactionRepository
    {
        public Task CreateTransactionsAsync(List<Transaction> transactions)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Transaction>> GetAllTransactionsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Transaction>> GetAllTransactionsByAccountIdAsync(string userId, string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> GetTransactionByIdAsync(string transactionId)
        {
            throw new NotImplementedException();
        }
    }
}
