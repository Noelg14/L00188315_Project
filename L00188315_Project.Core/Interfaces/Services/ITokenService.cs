using Microsoft.AspNetCore.Identity;

namespace L00188315_Project.Core.Interfaces.Services;

public interface ITokenService
{
    public string CreateToken(IdentityUser user);
}
