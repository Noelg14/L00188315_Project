using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L00188315_Project.Core.Entities
{
    /// <summary>
    /// Transaction entity representing a financial transaction associated with a bank account.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// ID of the transaction.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// Account Id associated with the transaction.
        /// </summary>
        public string? RootAccountId { get; set; }
        /// <summary>
        /// Account associated with the transaction.
        /// </summary>
        public Account? Account { get; set; } = null!;
        /// <summary>
        /// Amount of the transaction.
        /// </summary>
        public string? Amount { get; set; }
        /// <summary>
        /// Amount currency of the transaction.
        /// </summary>
        public string? AmountCurrency { get; set; }
        /// <summary>
        /// Credit or debit indicator of the transaction.
        /// </summary>
        public string? CreditDebitIndicator { get; set; }
        /// <summary>
        /// date and time when the transaction was created.
        /// </summary>
        public DateTime? BookingDateTime { get; set; }
        /// <summary>
        /// Value date and time of the transaction.
        /// </summary>
        public DateTime? ValueDateTime { get; set; }
        /// <summary>
        /// Creditor name associated with the transaction.
        /// </summary>
        public string? CreditorAccount { get; set; }
        /// <summary>
        /// Debitor name associated with the transaction.
        /// </summary>
        public string? DebtorAccount { get; set; }
        /// <summary>
        /// Proprietary bank transaction code associated with the transaction.
        /// </summary>
        public string? ProprietaryBankTransactionCode { get; set; }
        /// <summary>
        /// Proprietary bank transaction issuer associated with the transaction.
        /// </summary>
        public string? ProprietaryBankTransactionIssuer { get; set; }
        /// <summary>
        /// Status of the transaction.
        /// </summary>
        public string? Status { get; set; }
        /// <summary>
        /// Transaction Id associated with the transaction - from the bank
        /// </summary>
        public string? TransactionId { get; set; }
        /// <summary>
        /// Transaction information 
        /// </summary>
        public string? TransactionInformation { get; set; }
        /// <summary>
        /// User comments associated with the transaction.
        /// </summary>
        public string? UserComments { get; set; }
    }
}
