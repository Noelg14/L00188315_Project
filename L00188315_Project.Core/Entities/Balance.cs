using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L00188315_Project.Core.Entities
{
    /// <summary>
    /// Balance entity representing the balance of a bank account.
    /// </summary>
    public class Balance
    {
        /// <summary>
        /// Id of the Balance.
        /// </summary>
        public string BalanceId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Amount of the balance.
        /// </summary>
        public string? Amount { get; set; }

        /// <summary>
        /// Currency of the balance.
        /// </summary>
        public string? Currency { get; set; }

        /// <summary>
        /// Balance type
        /// </summary>
        public string? BalanceType { get; set; }

        /// <summary>
        /// Date and time when the balance was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Account Id associated with the balance.
        /// </summary>
        public string? RootAccountId { get; set; }

        /// <summary>
        /// Account associated with the balance.
        /// </summary>
        public Account? Account { get; set; } = null!;
    }
}
