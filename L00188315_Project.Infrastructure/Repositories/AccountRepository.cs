using L00188315_Project.Core.Entities;
using L00188315_Project.Core.Interfaces.Repositories;
using L00188315_Project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace L00188315_Project.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of the IAccountRepository interface for managing accounts.
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Constructor for AccountRepository.
        /// </summary>
        /// <param name="dbContext"></param>
        public AccountRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Create an account for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<Account> CreateAccountAsync(string userId, Account account)
        {
            account.UserId = userId;
            _dbContext.Accounts.Add(account);
            await _dbContext.SaveChangesAsync();
            return account;
        }

        /// <summary>
        /// Deletes a provided Account.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAccountAsync(string accountId)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == accountId);
            _dbContext.Accounts.Remove(account!);
            var complete = await _dbContext.SaveChangesAsync();
            return complete > 0;
        }

        /// <summary>
        /// Gets an account by its id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<Account> GetAccountAsync(string userId, string accountId)
        {
            var account = await _dbContext
                .Accounts.Where(x => x.AccountId == accountId && x.UserId == userId)
                .FirstOrDefaultAsync();
            return account!;
        }

        /// <summary>
        /// Gets all accounts for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Account>> GetAllAccountsAsync(string userId)
        {
            var accounts = await _dbContext
                .Accounts.Include(x => x.Balance)
                .Where(x => x.UserId == userId)
                .ToListAsync();
            return accounts;
        }

        /// <summary>
        /// Updates an account for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<Account> UpdateAccountAsync(string userId, Account account)
        {
            var currentAccount = await GetAccountAsync(userId, account.AccountId);
            if (currentAccount == null)
            {
                return await CreateAccountAsync(userId, account);
            }
            _dbContext.Accounts.Update(account);
            await _dbContext.SaveChangesAsync();
            return account;
        }
    }
}
