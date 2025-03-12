using System.Reflection.Metadata;

namespace L00188315_Project.Core.Entities
{
    public class Account
    {
        
        public string AccountId { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public string AccountType { get; set; }
        public string AccountSubType { get; set; }
        public string Iban { get; set; }
        public string SortCode { get; set; }
        public Balance? Balance { get; set; }
        public List<Transaction>? Transactions { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string? UserId { get; set; }
    }
}
