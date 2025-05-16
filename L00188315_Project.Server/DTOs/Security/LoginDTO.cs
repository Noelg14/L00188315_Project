using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace L00188315_Project.Server.DTOs.Security
{
    /// <summary>
    /// DTO for user login.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Email of user to be logged in
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Password of the user to be logged in
        /// </summary>
        public required string Password { get; set; }
    }
}
