using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L00188315_Project.Core.Entities;

namespace L00188315_Project.Core.Interfaces.Repositories
{
    /// <summary>
    /// Repository for managing balances
    /// </summary>
    public interface IBalanceRepository
    {
        /// <summary>
        /// Get all balances for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<List<Balance>> GetAllBalancesAsync(string userId);

        /// <summary>
        /// Get a balance by its id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public Task<Balance> GetBalanceAsync(string userId, string accountId);

        /// <summary>
        /// Create a balance
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        public Task<Balance> CreateBalanceAsync(string userId, Balance balance);

        /// <summary>
        /// Update a balance
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        public Task<Balance> UpdateBalanceAsync(string userId, Balance balance);
    }
}
