using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L00188315_Project.Core.Entities
{

    public class Consent
    {
        public string ConsentId { get; set; }
        public string? UserId { get; set; }
        public IdentityUser? User{ get; set; }
        public string? Scopes { get; set; }
        public string? Provider { get; set; }
        public ConsentStatus? ConsentStatus { get; set; } = Entities.ConsentStatus.Created;
    }
    public enum ConsentStatus { 
        Created,
        Failed,
        Complete
    }


}