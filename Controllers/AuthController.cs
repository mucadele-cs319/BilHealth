using BilHealth.Model.Dto.Incoming;
using BilHealth.Services.Users;
using BilHealth.Utility.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BilHealth.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService AuthenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            AuthenticationService = authenticationService;
        }

        [HttpPost]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Staff}")]
        public async Task<IActionResult> Register(RegistrationDto registration)
        {
            var result = await AuthenticationService.Register(registration);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LoginDto login)
        {
            var result = await AuthenticationService.LogIn(login);
            return result.Succeeded ? Ok(result) : Unauthorized(result);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await AuthenticationService.LogOut();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            var user = await AuthenticationService.GetUser(User);
            var result = await AuthenticationService.ChangePassword(user.AppUser, currentPassword, newPassword);

            return result.Succeeded ? Ok(result) : Unauthorized(result);
        }
    }
}
