using L00188315_Project.Core.Entities;
using L00188315_Project.Core.Interfaces.Repositories;
using L00188315_Project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace L00188315_Project.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of the IBalanceRepository interface for managing balances.
    /// </summary>
    /// <param name="_dbContext"></param>
    public class BalanceRepository(AppDbContext _dbContext) : IBalanceRepository
    {
        /// <summary>
        /// Create a balance for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        public async Task<Balance> CreateBalanceAsync(string userId, Balance balance)
        {
            _dbContext.Balances.Add(balance);
            await _dbContext.SaveChangesAsync();
            return balance;
        }

        /// <summary>
        /// Get All balances for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Balance>> GetAllBalancesAsync(string userId)
        {
            return await _dbContext.Balances.Where(b => b.Account!.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Get a balance by its linked account id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<Balance> GetBalanceAsync(string userId, string accountId)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _dbContext.Balances.FirstOrDefaultAsync(b =>
                b.Account!.AccountId == accountId && b.Account!.UserId == userId
            );
#pragma warning restore CS8603 // Possible null reference return.
            // it's okay to return null here, as we are checking for null in the service.
        }

        /// <summary>
        /// Updates a balance for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        public async Task<Balance> UpdateBalanceAsync(string userId, Balance balance)
        {
            _dbContext.Balances.Update(balance);
            await _dbContext.SaveChangesAsync();
            return balance;
        }
    }
}
