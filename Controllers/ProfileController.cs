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
            var user = await AuthenticationService.GetAppUser(base.User);
            var role = await AuthenticationService.GetUserRole(user);
            return DtoMapper.Map(user, role);
        }

        [HttpGet("{id:guid}")]
        public new async Task<IActionResult> User(Guid id)
        {
            // TODO: Constrain DTO fields according to the querying user's access level

            AppUser user;
            try
            {
                user = (await AuthenticationService.GetDomainUser(id)).AppUser;
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            var role = await AuthenticationService.GetUserRole(user);
            return Ok(DtoMapper.Map(user, role));
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserProfileDto newProfile)
        {
            var user = await AuthenticationService.GetAppUser(base.User);
            await ProfileService.UpdateProfile(user.DomainUser, newProfile);
            return Ok();
        }
    }
}
