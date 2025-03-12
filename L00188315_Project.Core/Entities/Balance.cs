using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L00188315_Project.Core.Entities
{
    public class Balance
    {
        public string? Amount { get; set; }
        public string? Currency { get; set; }
        public string? BalanceType { get; set; }
        public DateTime LastUpdated { get; set; }
        public string AccountId { get; set; }
        public Account Account { get; set; }
    }
}
