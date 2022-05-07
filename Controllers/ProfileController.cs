using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Model.Dto.Incoming;
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
        [Authorize(Roles = $"{UserType.Admin},{UserType.Staff}")]
        public async Task<List<SimpleUserDto>> GetAll()
        {
            var users = await AuthenticationService.GetAllUsers();
            return users.Select(DtoMapper.MapSimpleUser).ToList();
        }

        [HttpGet("me")]
        public async Task<UserProfileDto> GetCurrentUser()
        {
            var user = await AuthenticationService.GetUser(User);
            return DtoMapper.Map(user);
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var requestingUser = await AuthenticationService.GetUser(User);

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
        public async Task<IActionResult> Update(Guid userId, UserProfileUpdateDto details)
        {
            var requestingUser = await AuthenticationService.GetUser(User);
            await ProfileService.UpdateProfile(userId, details, requestingUser is Admin or Staff);
            return Ok();
        }

        [HttpPut("{patientUserId:guid}/blacklist")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Staff},{UserType.Nurse},{UserType.Doctor}")]
        public async Task SetBlacklist(Guid patientUserId, bool newState)
        {
            await ProfileService.SetPatientBlacklistState(patientUserId, newState);
        }

        [HttpPost("{patientUserId:guid}/vaccinations")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Staff},{UserType.Nurse},{UserType.Doctor}")]
        public async Task AddVaccination(Guid patientUserId, VaccinationUpdateDto details)
        {
            await ProfileService.AddVaccination(patientUserId, details);
        }

        [HttpPut("vaccinations/{vaccinationId:guid}")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Staff},{UserType.Nurse},{UserType.Doctor}")]
        public async Task UpdateVaccination(Guid vaccinationId, VaccinationUpdateDto details)
        {
            await ProfileService.UpdateVaccination(vaccinationId, details);
        }

        [HttpDelete("vaccinations/{vaccinationId:guid}")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Staff},{UserType.Nurse},{UserType.Doctor}")]
        public async Task RemoveVaccination(Guid vaccinationId)
        {
            await ProfileService.RemoveVaccination(vaccinationId);
        }
    }
}
