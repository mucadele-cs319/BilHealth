using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Services;
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
    public class AppointmentController : ControllerBase
    {
        private readonly IAuthenticationService AuthenticationService;
        private readonly IAppointmentService AppointmentService;

        public AppointmentController(IAuthenticationService authenticationService, IAppointmentService appointmentService)
        {
            AuthenticationService = authenticationService;
            AppointmentService = appointmentService;
        }

        [HttpPost]
        public async Task<AppointmentDto> Create(AppointmentDto details)
        {
            details.RequestedById = (await AuthenticationService.GetAppUser(User)).DomainUser.Id;
            var appointment = await AppointmentService.CreateAppointmentRequest(details);
            return DtoMapper.Map(appointment);
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Nurse},{UserRoleType.Constant.Doctor}")]
        public async Task<AppointmentVisitDto> CreateVisit(AppointmentVisitDto details)
        {
            var visit = await AppointmentService.CreateVisit(details);
            return DtoMapper.Map(visit);
        }

        [HttpPatch]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Staff},{UserRoleType.Constant.Nurse},{UserRoleType.Constant.Doctor}")]
        public async Task SetApproval(Guid appointmentId, ApprovalStatus approval)
        {
            await AppointmentService.SetAppointmentApproval(appointmentId, approval);
        }

        [HttpPatch]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Staff},{UserRoleType.Constant.Nurse},{UserRoleType.Constant.Doctor}")]
        public async Task UpdateVisit(AppointmentVisitDto details)
        {
            await AppointmentService.UpdatePatientVisitDetails(details);
        }

        [HttpPatch]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Staff},{UserRoleType.Constant.Nurse},{UserRoleType.Constant.Doctor}")]
        public async Task SetBlacklist(Guid patientUserId, bool newState)
        {
            await AppointmentService.SetPatientBlacklistState(patientUserId, newState);
        }
    }
}
