using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Model.Dto.Incoming;
using BilHealth.Services.AccessControl;
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
        private readonly IAccessControlService AccessControlService;
        private readonly IProfileService ProfileService;

        public ProfileController(IAuthenticationService authenticationService, IAccessControlService accessControlService, IProfileService profileService)
        {
            AuthenticationService = authenticationService;
            AccessControlService = accessControlService;
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

            DomainUser user;
            try
            {
                user = await AuthenticationService.GetUser(userId);
            }
            catch (IdNotFoundException) { return NotFound(); }

            return await AccessControlService.AccessGuard(requestingUser.Id, user.Id) ? Ok(DtoMapper.Map(user)) : Forbid();
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

        [HttpPost("{patientUserId:guid}/accessgrants")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Staff},{UserType.Patient}")]
        public async Task<IActionResult> GrantAccess(Guid patientUserId, TimedAccessGrantCreateDto details)
        {
            var requestingUser = await AuthenticationService.GetUser(User);
            if (!await AccessControlService.Profile(requestingUser.Id, patientUserId)) return Forbid();

            await AccessControlService.GrantTimedAccess(details);
            return Ok();
        }

        [HttpPatch("{patientUserId:guid}/accessgrants/{accessGrantId:guid}")]
        public async Task<IActionResult> CancelAccessGrant(Guid patientUserId, Guid accessGrantId)
        {
            var requestingUser = await AuthenticationService.GetUser(User);
            if (!await AccessControlService.Profile(requestingUser.Id, patientUserId)) return Forbid();

            try
            {
                await AccessControlService.CancelTimedAccessGrant(accessGrantId);
            }
            catch (IdNotFoundException) { return NotFound(); }
            return Ok();
        }

        [HttpGet("audittrails")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Staff}")]
        public async Task<List<AuditTrailDto>> GetRecentAuditTrails(Guid patientUserId, TimedAccessGrantCreateDto details)
        {
            return (await AccessControlService.GetRecentAuditTrails()).Select(DtoMapper.Map).ToList();
        }
    }
}
