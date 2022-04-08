using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Services.Users;
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
            var domainUser = user.DomainUser;
            var role = await AuthenticationService.GetUserRole(user);

            var dto = new UserProfileDto
            {
                UserType = role,
                Email = user.Email,
                FirstName = domainUser.FirstName,
                LastName = domainUser.LastName,
                Gender = domainUser.Gender,
                DateOfBirth = domainUser.DateOfBirth
            };

            if (domainUser is Patient patient)
            {
                dto.BodyWeight = patient.BodyWeight;
                dto.BodyHeight = patient.BodyHeight;
                dto.BloodType = patient.BloodType;
                dto.Vaccinations = patient.Vaccinations;
                dto.TestResults = patient.TestResults;
                dto.Cases = patient.Cases;
                dto.Blacklisted = patient.Blacklisted;
            }
            else if (domainUser is Nurse nurse)
            {
                dto.TriageRequests = nurse.TriageRequests;
            }
            else if (domainUser is Doctor doctor)
            {
                dto.Specialization = doctor.Specialization;
                dto.Campus = doctor.Campus;
                dto.Cases = doctor.Cases;
            }

            return dto;
        }
    }
}
