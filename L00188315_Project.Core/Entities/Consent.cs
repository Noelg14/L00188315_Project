using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace L00188315_Project.Core.Entities
{
    /// <summary>
    /// Consent entity representing the user's consent for data sharing.
    /// </summary>
    public class Consent
    {
        /// <summary>
        /// ID of the consent.
        /// </summary>
        public required string ConsentId { get; set; }
        /// <summary>
        /// User ID associated with the consent.
        /// </summary>
        public string? UserId { get; set; }
        /// <summary>
        /// Scopes of the consent.
        /// </summary>
        public string? Scopes { get; set; }
        /// <summary>
        /// Provider of the consent.
        /// </summary>
        public string? Provider { get; set; }
        /// <summary>
        /// Conent status.
        /// </summary>
        public ConsentStatus? ConsentStatus { get; set; } = Entities.ConsentStatus.Created;
        /// <summary>
        /// List of accounts associated with the consent.
        /// </summary>
        public List<Account>? Account { get; set; }
        /// <summary>
        /// Date and time when the consent was created.
        /// </summary>
        public DateTime? Created { get; set; } = DateTime.Now;
        /// <summary>
        ///  Date and time when the consent was last updated.
        /// </summary>
        public DateTime? Updated { get; set; }
        /// <summary>
        /// Da and time when the consent expires.
        /// </summary>
        public DateTime? Expires { get; set; }
    }
    /// <summary>
    /// Enum representing the status of the consent.
    /// </summary>
    public enum ConsentStatus
    {
        Created,
        Pending,
        Failed,
        Complete,
        Expired,
    }
}
