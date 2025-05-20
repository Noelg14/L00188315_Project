using Microsoft.AspNetCore.Identity;

namespace L00188315_Project.Core.Interfaces.Services;
/// <summary>
/// Token service interface. Allows for changing the token service implementation without changing the code that uses it.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Creates a token for the user. The token is a JWT token that contains the user's claims and is signed with the secret key.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public string CreateToken(IdentityUser user);
}
