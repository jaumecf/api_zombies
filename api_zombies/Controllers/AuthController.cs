using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api_zombies.Models;
using api_zombies.Services.Authentication;
using System.Security.Claims;

namespace api_zombies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Usuari>> Register(UserDto request)
        {
            bool result = await _authService.ExistsUserAsync(request);
            if (result)
                return BadRequest("Error: email already registered.");

            var newUser = await _authService.RegisterAsync(request, false);
            return Ok(newUser);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<Usuari>> Login(UserDto request)
        {
            bool existUser = await _authService.ExistsUserAsync(request);
            if (!existUser)
                return BadRequest("Error: User not found.");

            bool validPassword = await _authService.VerifyPasswordHashAsync(request);
            if (!validPassword)
            {
                return BadRequest("Wrong password.");
            }

            return Ok(await _authService.LoginAsync(request));
        }

        [HttpGet("GetMe"), Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = User?.Identity?.Name;
            //var userName2 = User.FindFirstValue(ClaimTypes.Name);
            var role = User?.FindFirstValue(ClaimTypes.Role);
            var email = User?.FindFirstValue(ClaimTypes.Email);
            var id = User?.FindFirstValue(ClaimTypes.Sid);

            return Ok(new { userName, role, email, id });
        }
    }
}
