using System.Reflection.Metadata;

namespace L00188315_Project.Core.Entities
{
    /// <summary>
    /// Entity representing a bank account.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Id of the account.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// Account Id from the bank
        /// </summary>
        public required string AccountId { get; set; }
        /// <summary>
        /// Name of the account.
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Currency of the account.
        /// </summary>
        public required string Currency { get; set; }
        /// <summary>
        /// Type of the account.
        /// </summary>
        public required string AccountType { get; set; }
        /// <summary>
        /// Account sub-type.
        /// </summary>
        public required string AccountSubType { get; set; }
        /// <summary>
        /// International Bank Account Number (IBAN).
        /// </summary>
        public required string Iban { get; set; }
        /// <summary>
        /// Sort code of the account.
        /// </summary>
        public string? SortCode { get; set; }
        /// <summary>
        /// Balance of the Account.
        /// </summary>
        public Balance? Balance { get; set; }
        /// <summary>
        /// Collection of transactions associated with the account.
        /// </summary>
        public ICollection<Transaction>? Transactions { get; set; }
        /// <summary>
        /// Date and time when the account was added to the system.
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Date and time when the account was last updated.
        /// </summary>
        public DateTime Updated { get; set; }
        /// <summary>
        /// User Id associated with the account.
        /// </summary>
        public string? UserId { get; set; }
    }
}
