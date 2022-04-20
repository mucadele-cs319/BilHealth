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
    [Route("api/[controller]s")]
    [Authorize]
    [Produces("application/json")]
    public class ProfileController : ControllerBase
    {
        private readonly IAuthenticationService AuthenticationService;
        private readonly IProfileService ProfileService;

        public ProfileController(IAuthenticationService authenticationService, IProfileService profileService)
        {
            AuthenticationService = authenticationService;
            ProfileService = profileService;
        }

        [HttpGet("me")]
        public async Task<UserProfileDto> GetCurrentUser()
        {
            var user = await AuthenticationService.GetAppUser(base.User);
            var role = await AuthenticationService.GetUserRole(user);
            return DtoMapper.Map(user, role);
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            // TODO: Constrain DTO fields according to the querying user's access level

            AppUser user;
            try
            {
                user = (await AuthenticationService.GetDomainUser(userId)).AppUser;
            }
            catch (IdNotFoundException)
            {
                return NotFound();
            }

            var role = await AuthenticationService.GetUserRole(user);
            return Ok(DtoMapper.Map(user, role));
        }

        [HttpPatch("{userId:guid}")]
        public async Task<IActionResult> Update(Guid userId, UserProfileDto newProfile)
        {
            await ProfileService.UpdateProfile(userId, newProfile);
            return Ok();
        }

        [HttpPut("{patientUserId:guid}/blacklist")]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Staff},{UserRoleType.Constant.Nurse},{UserRoleType.Constant.Doctor}")]
        public async Task SetBlacklist(Guid patientUserId, bool newState)
        {
            await ProfileService.SetPatientBlacklistState(patientUserId, newState);
        }
    }
}
