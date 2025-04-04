using L00188315_Project.Core.Entities;
using L00188315_Project.Core.Interfaces.Repositories;
using L00188315_Project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;


namespace L00188315_Project.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _dbContext;
        public AccountRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Account> CreateAccountAsync(string userId, Account account)
        {
            account.UserId = userId;
            _dbContext.Accounts.Add(account);
            await _dbContext.SaveChangesAsync();
            return account; 

        }

        public async Task<Account> GetAccountAsync(string userId, string accountId)
        {
            var account = await _dbContext.Accounts
              .Where(x => x.AccountId == accountId && x.UserId == userId).FirstOrDefaultAsync();
            return account!;
        }

        public async Task<List<Account>> GetAllAccountsAsync(string userId)
        {
            var accounts = await _dbContext.Accounts
              .Where(x => x.UserId == userId).ToListAsync();
            return accounts;
        }

        public async Task<Account> UpdateAccountAsync(string userId, Account account)
        {
            var currentAccount = await GetAccountAsync(userId, account.AccountId);
            if(currentAccount == null)
            {
                return await CreateAccountAsync(userId, account);
            }
            _dbContext.Accounts.Update(account);
            await _dbContext.SaveChangesAsync();
            return account;
        }
    }
}
