using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L00188315_Project.Core.Entities
{
    public class Transaction
    {
        public string? RootAccountId { get; set; }
        public Account? Account { get; set; } = null!;
        public string? Amount { get; set; }
        public string? AmountCurrency { get; set; }
        public string? CreditDebitIndicator { get; set; }
        public DateTime? BookingDateTime { get; set; }
        public DateTime? ValueDateTime { get; set; }
        public string? CreditorAccount { get; set; }
        public string? DebtorAccount { get; set; }
        public string? ProprietaryBankTransactionCode { get; set; }
        public string? ProprietaryBankTransactionIssuer { get; set; }
        public string? Status { get; set; }
        public string? TransactionId { get; set; }
        public string? TransactionInformation { get; set; }
        public string? UserComments { get; set; }
    }
}
