using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace L00188315_Project.Core.Entities
{
    public class Consent
    {
        public required string ConsentId { get; set; }
        public string? UserId { get; set; }
        public string? Scopes { get; set; }
        public string? Provider { get; set; }
        public ConsentStatus? ConsentStatus { get; set; } = Entities.ConsentStatus.Created;
        public List<Account>? Account { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }
        public DateTime? Expires { get; set; }
    }

    public enum ConsentStatus
    {
        Created,
        Pending,
        Failed,
        Complete,
        Expired,
    }
}
