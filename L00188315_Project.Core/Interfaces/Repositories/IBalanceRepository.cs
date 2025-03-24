using L00188315_Project.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L00188315_Project.Core.Interfaces.Repositories
{
    public interface IBalanceRepository
    {
        public Task<List<Balance>> GetAllBalancesAsync(string userId);
        public Task<Balance> GetBalanceAsync(string userId, string accountId);
        public Task<Balance> CreateBalanceAsync(string userId, Balance balance);
        public Task<Balance> UpdateBalanceAsync(string userId, Balance balance);

    }
}
