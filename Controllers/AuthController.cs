using BilHealth.Model.Transaction;
using BilHealth.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BilHealth.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService AuthenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            AuthenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register registration)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(Login login)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            throw new NotImplementedException();
        }
    }
}
