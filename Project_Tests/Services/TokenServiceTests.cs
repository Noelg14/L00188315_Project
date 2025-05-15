using System.IdentityModel.Tokens.Jwt;
using System.Text;
using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace Project_Tests;

public class TokenServiceTests
{
    private readonly ITokenService _tokenService;
    private readonly Mock<IConfiguration> _configuration;

    public TokenServiceTests()
    {
        _configuration = new Mock<IConfiguration>();
        _configuration
            .Setup(x => x["Token:Key"])
            .Returns("rjkq56RxW41yLcKSZn8trUD2qaCbiZQgcyg53DgkKR58ezBRpAjziKUBnSYnQRzU"); // random key
        _tokenService = new TokenService(_configuration.Object);
    }

    [Fact]
    public void CreateToken_For_Valid_User_Returns_Token()
    {
        //Arrange
        var user = new IdentityUser
        {
            Id = "1",
            Email = "email@email.ie",
            UserName = "username",
        };
        //Act
        var token = _tokenService.CreateToken(user);
        //Assert
        Assert.NotNull(token);
    }

    [Fact]
    public void CreateToken_Uses_Correct_Key()
    {
        //Arrange
        var user = new IdentityUser
        {
            Id = "1",
            Email = "email@email.ie",
            UserName = "username",
        };
        //Act
        var token = _tokenService.CreateToken(user);
        SecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.Object["Token:Key"])
            ),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
        var principal = tokenHandler.ValidateToken(
            token,
            tokenValidationParameters,
            out SecurityToken validatedToken
        );
        var validToken = validatedToken as JwtSecurityToken;
        //Assert
        Assert.NotNull(validToken);
    }
}
