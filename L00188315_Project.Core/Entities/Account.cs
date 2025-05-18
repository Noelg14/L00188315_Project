using System.Reflection.Metadata;

namespace L00188315_Project.Core.Entities
{
    public class Account
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string AccountId { get; set; }
        public required string Name { get; set; }
        public required string Currency { get; set; }
        public required string AccountType { get; set; }
        public required string AccountSubType { get; set; }
        public required string Iban { get; set; }
        public string? SortCode { get; set; }
        public Balance? Balance { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string? UserId { get; set; }
    }
}
