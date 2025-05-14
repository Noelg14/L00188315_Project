using L00188315_Project.Core.Interfaces.Services;
using L00188315_Project.Server.DTOs.Security;
using Microsoft.AspNetCore.Authorization;
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
        SignInManager<IdentityUser> _signInManager,
        ITokenService _tokenService
    ) : ControllerBase
    {
        /// <summary>
        /// Validates a users credentials against the ID database
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns>a <see cref="UserDTO"/> with a valid JWT</returns>
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Auth([FromBody] LoginDTO loginDTO)
        {
            ////User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null)
                return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                loginDTO.Password,
                false
            );

            if (!result.Succeeded)
                return Unauthorized();

            return new UserDTO
            {
                Email = user.Email,
                DisplayName = user.UserName,
                Token = _tokenService.CreateToken(user),
                UserId = user.Id,
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (existingUser is not null)
                return BadRequest("User already exists");
                
            var user = new IdentityUser { UserName = registerDTO.Name, Email = registerDTO.Email };
            var result = await _userManager.CreateAsync(user, registerDTO.Password!);
            if (!result.Succeeded)
                return BadRequest("Account could not be created");
            return new UserDTO
            {
                Email = user.Email,
                DisplayName = user.UserName,
                Token = _tokenService.CreateToken(user),
                UserId = user.Id,
            };
        }
        [HttpGet("me")]
        public async Task<ActionResult<UserDTO>> GetLoggedInUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            return new UserDTO
            {
                Email = user.Email,
                DisplayName = user.UserName,
                Token = _tokenService.CreateToken(user),
                UserId = user.Id
            };
        } 

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}
