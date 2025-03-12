using L00188315_Project.Server.DTOs.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace L00188315_Project.Server.Controllers
{
    [Route("api/")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Auth([FromBody] LoginDTO loginDTO)
        {
            return Ok(); ;
        }
    }
}
