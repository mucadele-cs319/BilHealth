using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Services.Users;
using BilHealth.Utility;
using BilHealth.Utility.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BilHealth.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IAuthenticationService AuthenticationService;
        private readonly IProfileService ProfileService;

        public ProfileController(IAuthenticationService authenticationService, IProfileService profileService)
        {
            AuthenticationService = authenticationService;
            ProfileService = profileService;
        }

        [HttpGet]
        public async Task<UserProfileDto> Me()
        {
            var user = await AuthenticationService.GetUser(User);
            var role = await AuthenticationService.GetUserRole(user);
            return DtoMapper.Map(user, role);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserProfileDto newProfile)
        {
            var user = await AuthenticationService.GetUser(User);
            await ProfileService.UpdateProfile(user.DomainUser, newProfile);
            return Ok();
        }
    }
}
