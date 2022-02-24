using BilHealth.Model.Dto;
using BilHealth.Services.Users;
using BilHealth.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BilHealth.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService AuthenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            AuthenticationService = authenticationService;
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Register(Registration registration)
        {
            var result = await AuthenticationService.Register(registration);

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(Login login)
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
            var user = await AuthenticationService.getUser(User);
            var result = await AuthenticationService.ChangePassword(user, currentPassword, newPassword);

            return result.Succeeded ? Ok(result) : Unauthorized(result);
        }
    }
}
