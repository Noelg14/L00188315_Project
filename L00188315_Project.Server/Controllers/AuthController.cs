using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Server.DTOs.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace L00188315_Project.Server.Controllers
{
    [Route("api/")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController(
        UserManager<IdentityUser> _userManager, 
        SignInManager<IdentityUser> _signInManager
        ) : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Auth([FromBody] LoginDTO loginDTO)
        {
            //User.FindFirstValue(ClaimTypes.Email);
            return Ok();
            /*
                var user = await _userManager.FindByEmailAsync (loginDto.Email);

                if (user == null) return Unauthorized (new ApiResponse (401));

                var result = await _signInManager.CheckPasswordSignInAsync (user, loginDto.Password, false);

                if (!result.Succeeded) return Unauthorized (new ApiResponse (401));

                return new UserDto {
                    Email = user.Email,
                        DisplayName = user.DisplayName,
                        Token = _tokenService.CreateToken (user)
                };
            */
        }
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}
