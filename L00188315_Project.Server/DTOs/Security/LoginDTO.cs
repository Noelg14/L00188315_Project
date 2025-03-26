using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace L00188315_Project.Server.DTOs.Security
{
    public class LoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
