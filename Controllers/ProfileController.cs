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

        [HttpGet]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Staff}")]
        public async Task<List<SimpleUserDto>> GetAll()
        {
            var users = await AuthenticationService.GetAllAppUsers();
            return users.Select(u => DtoMapper.MapSimpleUser(u.DomainUser)).ToList();
        }

        [HttpGet("me")]
        public async Task<UserProfileDto> GetCurrentUser()
        {
            var user = await AuthenticationService.GetAppUser(User);
            return DtoMapper.Map(user.DomainUser);
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            DomainUser requestingUser = (await AuthenticationService.GetAppUser(User)).DomainUser;

            UserProfileDto dto;
            try
            {
                dto = await ProfileService.GetFilteredUser(requestingUser, userId);
            }
            catch (IdNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException)
            {
                return Forbid();
            }

            return Ok(dto);
        }

        [HttpPatch("{userId:guid}")]
        public async Task<IActionResult> Update(Guid userId, UserProfileDto newProfile)
        {
            var requestingUser = (await AuthenticationService.GetAppUser(User)).DomainUser;
            await ProfileService.UpdateProfile(userId, newProfile, requestingUser is Admin or Staff);
            return Ok();
        }

        [HttpPut("{patientUserId:guid}/blacklist")]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Staff},{UserRoleType.Constant.Nurse},{UserRoleType.Constant.Doctor}")]
        public async Task SetBlacklist(Guid patientUserId, bool newState)
        {
            await ProfileService.SetPatientBlacklistState(patientUserId, newState);
        }

        [HttpPost("{patientUserId:guid}/vaccinations")]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Staff},{UserRoleType.Constant.Nurse},{UserRoleType.Constant.Doctor}")]
        public async Task AddVaccination(Guid patientUserId, VaccinationDto details)
        {
            await ProfileService.AddVaccination(details);
        }

        [HttpPut("vaccinations/{vaccinationId:guid}")]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Staff},{UserRoleType.Constant.Nurse},{UserRoleType.Constant.Doctor}")]
        public async Task UpdateVaccination(Guid vaccinationId, VaccinationDto details)
        {
            details.Id = vaccinationId;
            await ProfileService.UpdateVaccination(details);
        }

        [HttpDelete("vaccinations/{vaccinationId:guid}")]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Staff},{UserRoleType.Constant.Nurse},{UserRoleType.Constant.Doctor}")]
        public async Task RemoveVaccination(Guid vaccinationId)
        {
            await ProfileService.RemoveVaccination(vaccinationId);
        }
    }
}
