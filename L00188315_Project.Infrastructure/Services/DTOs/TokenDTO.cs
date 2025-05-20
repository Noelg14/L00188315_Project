using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L00188315_Project.Infrastructure.Services.DTOs
{
    /// <summary>
    /// KeyVault token DTO
    /// </summary>
    class TokenDTO
    {
        /// <summary>
        /// Access token for KeyVault
        /// </summary>
        public required string access_token { get; set; }
        /// <summary>
        /// Token type for KeyVault - usually "Bearer"
        /// </summary>
        public required string token_type { get; set; }
        /// <summary>
        /// Expiration time for the token in seconds
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// Expiration time for the token in seconds (extended)
        /// </summary>
        public int? ext_expires_in { get; set; }
    }
}
