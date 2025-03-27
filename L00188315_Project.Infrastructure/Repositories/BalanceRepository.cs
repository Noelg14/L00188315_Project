using L00188315_Project.Core.Entities;
using L00188315_Project.Core.Interfaces.Repositories;
using L00188315_Project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace L00188315_Project.Infrastructure.Repositories
{
    public class BalanceRepository(AppDbContext _dbContext) : IBalanceRepository
    {

        public async Task<Balance> CreateBalanceAsync(string userId, Balance balance)
        {
            _dbContext.Balances.Add(balance);
            await _dbContext.SaveChangesAsync();
            return balance;
        }

        public async Task<List<Balance>> GetAllBalancesAsync(string userId)
        {
            return await _dbContext.Balances.Where(b => b.Account.UserId == userId).ToListAsync();
        }

        public async Task<Balance> GetBalanceAsync(string userId, string accountId)
        {
            return _dbContext.Balances.Where(b => b.AccountId == accountId && b.Account.UserId == userId).FirstOrDefault();
        }

        public async Task<Balance> UpdateBalanceAsync(string userId, Balance balance)
        {
            _dbContext.Balances.Update(balance);
            await _dbContext.SaveChangesAsync();
            return balance;
        }
    }
}
