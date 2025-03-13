using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace L00188315_Project.Core.Models
{
    public class OpenBankingDataModel
    {
        public Data? Data { get; set; }
        public Risk? Risk { get; set; }
        public Links? Links { get; set; }
        public Meta? Meta { get; set; }
    }
    public class Data
    {
        public List<Account>? Account { get; set; }
        public List<Transaction>? Transaction { get; set; }
        public List<Balance>? Balance { get; set; }
        public List<string>? Permissions { get; set; }
        public DateTime? ExpirationDateTime { get; set; }
        public DateTime? TransactionFromDateTime { get; set; }
        public DateTime? TransactionToDateTime { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public DateTime? StatusUpdateDateTime { get; set; }
        public string? ConsentId { get; set; }
        public string? Status { get; set; }

    }
    public class Risk { }
    public class Links
    {
        public string? Self { get; set; }
        public string? Next { get; set; }
    }
    public class Meta
    {
        public int TotalPages { get; set; }
    }

    public class Account
    {
        public string? AccountId { get; set; }
        public string? Currency { get; set; }
        public string? AccountType { get; set; }
        public string? AccountSubType { get; set; }
        public string? SchemeName { get; set; }
        public string? Identification { get; set; }
        public string? Name { get; set; }
        [JsonPropertyName("Account")]
        public List<Account>? _Account { get; set; }

    }
    public class Transaction
    {
        public string? AccountId { get; set; }
        public Amount? Amount { get; set; }
        //public Balance? Balance { get; set; }
        public string? CreditDebitIndicator { get; set; }
        public DateTime? BookingDateTime { get; set; }
        public DateTime? ValueDateTime { get; set; }
        public CurrencyExchange? CurrencyExchange { get; set; }
        public Account? CreditorAccount { get; set; }
        public Account? DebtorAccount { get; set; }
        public ProprietaryBankTransactionCode? ProprietaryBankTransactionCode { get; set; }
        public string? Status { get; set; }
        public string? TransctionId { get; set; }
        public string? TransactionInformation { get; set; }
        public SupplementaryData? SupplementaryData { get; set; }

    }
    public class Amount
    {
        [JsonPropertyName("Amount")]
        public string? _Amount { get; set; }
        public string? Currency { get; set; }
    }
    public class Balance
    {
        public Amount? Amount { get; set; }
        public string? CreditDebitIndicator { get; set; }
        public string? Type { get; set; }
        public string? DateTime { get; set; }
        public string? AccountId { get; set; }

    }
    public class CurrencyExchange
    {
        public Amount? InsctructedAmount { get; set; }
        public string? SourceCurrency { get; set; }
        public string? TargetCurrency { get; set; }
        public string? UnitCurrency { get; set; }
        public string? ExchangeRate { get; set; }
    }
    public class ProprietaryBankTransactionCode
    {
        public string? Code { get; set; }
        public string? Issuer { get; set; }
    }
    public class SupplementaryData
    {
        public string? UserComments { get; set; }
    }


}
