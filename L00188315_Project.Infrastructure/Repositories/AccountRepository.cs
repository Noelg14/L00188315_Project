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
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _dbContext;
        public AccountRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Account> CreateAccountAsync(string userId, Account account)
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetAccountAsync(string userId, string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Account>> GetAllAccountsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Account> UpdateAccountAsync(string userId, Account account)
        {
            throw new NotImplementedException();
        }
    }
}
