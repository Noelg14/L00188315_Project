using System.Text.Json.Serialization;

namespace L00188315_Project.Core.Models
{
    /// <summary>
    /// Open Banking Data Model
    /// </summary>
    public class OpenBankingDataModel
    {
        /// <summary>
        /// Data returned from the Open Banking API
        /// </summary>
        public Data? Data { get; set; }

        /// <summary>
        /// Risk information returned from the Open Banking API
        /// </summary>
        public Risk? Risk { get; set; }

        /// <summary>
        /// Links to the Open Banking API
        /// </summary>
        public Links? Links { get; set; }

        /// <summary>
        /// Meta information returned from the Open Banking API
        /// </summary>
        public Meta? Meta { get; set; }
    }

    /// <summary>
    /// Data returned from the Open Banking API
    /// </summary>
    public class Data
    {
        /// <summary>
        /// List of accounts returned from the Open Banking API
        /// </summary>
        [JsonPropertyName("Account")]
        public List<OBAccount>? Account { get; set; }

        /// <summary>
        /// List of transactions returned from the Open Banking API
        /// </summary>
        [JsonPropertyName("Transaction")]
        public List<OBTransaction>? Transaction { get; set; }

        /// <summary>
        /// List of balances returned from the Open Banking API
        /// </summary>
        [JsonPropertyName("Balance")]
        public List<OBBalance>? Balance { get; set; }

        /// <summary>
        /// List of permissions returned from the Open Banking API
        /// </summary>
        public List<string>? Permissions { get; set; }

        /// <summary>
        /// Expiration date and time of the Data
        /// </summary>
        public DateTime? ExpirationDateTime { get; set; }

        /// <summary>
        /// Start date and time of the Transaction data
        /// </summary>
        public DateTime? TransactionFromDateTime { get; set; }

        /// <summary>
        /// Transaction end date and time
        /// </summary>
        public DateTime? TransactionToDateTime { get; set; }

        /// <summary>
        /// Creation date and time of the Data
        /// </summary>
        public DateTime? CreationDateTime { get; set; }

        /// <summary>
        /// Status update date and time of the Data
        /// </summary>
        public DateTime? StatusUpdateDateTime { get; set; }

        /// <summary>
        /// Consent ID
        /// </summary>
        public string? ConsentId { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string? Status { get; set; }
    }

    /// <summary>
    /// Risk information returned from the Open Banking API
    /// </summary>
    public class Risk { }

    /// <summary>
    /// Links to the Open Banking API
    /// </summary>
    public class Links
    {
        /// <summary>
        /// Link of the current resource
        /// </summary>
        public string? Self { get; set; }

        /// <summary>
        /// Link of the next resource
        /// </summary>
        public string? Next { get; set; }
    }

    /// <summary>
    /// Meta information returned from the Open Banking API
    /// </summary>
    public class Meta
    {
        /// <summary>
        /// Total number of pages returned from the Open Banking API
        /// </summary>
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// Object representing an account in the Open Banking API
    /// </summary>
    public class OBAccount
    {
        /// <summary>
        /// Account ID of the account
        /// </summary>
        public string? AccountId { get; set; }

        /// <summary>
        /// Currency of the account
        /// </summary>
        public string? Currency { get; set; }

        /// <summary>
        /// Account Type of the account
        /// </summary>
        public string? AccountType { get; set; }

        /// <summary>
        /// Account Sub Type of the account
        /// </summary>
        public string? AccountSubType { get; set; }

        /// <summary>
        /// Scheme Name of the account
        /// </summary>
        public string? SchemeName { get; set; }

        /// <summary>
        /// Account Identification
        /// </summary>
        public string? Identification { get; set; }

        /// <summary>
        /// Name of the account holder
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Accounts linked to this account
        /// </summary>
        [JsonPropertyName("Account")]
        public List<OBAccount>? _Account { get; set; }
    }

    /// <summary>
    /// Object representing a transaction in the Open Banking API
    /// </summary>
    public class OBTransaction
    {
        /// <summary>
        /// Account ID of the transaction
        /// </summary>
        public string? AccountId { get; set; }

        /// <summary>
        /// Amount of money in the transaction
        /// </summary>
        public Amount? Amount { get; set; }

        /// <summary>
        /// Credit or debit indicator of the transaction
        /// </summary>
        public string? CreditDebitIndicator { get; set; }

        /// <summary>
        /// Date and time of the transaction
        /// </summary>
        public DateTime? BookingDateTime { get; set; }

        /// <summary>
        /// Date and time of the value of the transaction
        /// </summary>
        public DateTime? ValueDateTime { get; set; }

        /// <summary>
        /// Currency exchange information of the transaction
        /// </summary>
        public CurrencyExchange? CurrencyExchange { get; set; }

        /// <summary>
        /// Creditor information of the transaction
        /// </summary>
        public OBAccount? CreditorAccount { get; set; }

        /// <summary>
        /// Debtor information of the transaction
        /// </summary>
        public OBAccount? DebtorAccount { get; set; }

        /// <summary>
        /// Proprietary bank transaction code of the transaction
        /// </summary>
        public ProprietaryBankTransactionCode? ProprietaryBankTransactionCode { get; set; }

        /// <summary>
        /// Status of the transaction
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Transaction Id of the transaction
        /// </summary>
        public string? TransactionId { get; set; }

        /// <summary>
        /// Transaction Information
        /// </summary>
        public string? TransactionInformation { get; set; }

        /// <summary>
        /// Supplementary data of the transaction
        /// </summary>
        public SupplementaryData? SupplementaryData { get; set; }
    }

    /// <summary>
    /// Amount object representing the amount of money in a transaction or balance
    /// </summary>
    public class Amount
    {
        /// <summary>
        /// Amount of money in the transaction or balance
        /// </summary>
        [JsonPropertyName("Amount")]
        public string? _Amount { get; set; }

        /// <summary>
        /// Currency of the amount
        /// </summary>
        public string? Currency { get; set; }
    }

    /// <summary>
    /// Balance object representing the balance of an account
    /// </summary>
    public class OBBalance
    {
        /// <summary>
        /// Amount of money in the account
        /// </summary>
        public Amount? Amount { get; set; }

        /// <summary>
        /// Credit or debit indicator of the account
        /// </summary>
        public string? CreditDebitIndicator { get; set; }

        /// <summary>
        /// Type of balance
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Date and time of the balance
        /// </summary>
        public string? DateTime { get; set; }

        /// <summary>
        /// Account ID of the balance
        /// </summary>
        public string? AccountId { get; set; }
    }

    /// <summary>
    /// Currency exchange object representing the currency exchange information of a transaction
    /// </summary>
    public class CurrencyExchange
    {
        /// <summary>
        /// Amount of money in the transaction
        /// </summary>
        public Amount? InsctructedAmount { get; set; }

        /// <summary>
        /// Source currency of the transaction
        /// </summary>
        public string? SourceCurrency { get; set; }

        /// <summary>
        /// Target currency of the transaction
        /// </summary>
        public string? TargetCurrency { get; set; }

        /// <summary>
        /// Unit currency of the transaction
        /// </summary>
        public string? UnitCurrency { get; set; }

        /// <summary>
        /// Exchange rate of the transaction
        /// </summary>
        public string? ExchangeRate { get; set; }
    }

    /// <summary>
    /// Proprietary bank transaction code object representing the proprietary bank transaction code of a transaction
    /// </summary>
    public class ProprietaryBankTransactionCode
    {
        /// <summary>
        /// Proprietary bank transaction code
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Proprietary bank transaction code issuer
        /// </summary>
        public string? Issuer { get; set; }
    }

    /// <summary>
    /// Supplementary data object representing the supplementary data of a transaction
    /// </summary>
    public class SupplementaryData
    {
        /// <summary>
        /// User comments related to the transaction
        /// </summary>
        public string? UserComments { get; set; }
    }
}
