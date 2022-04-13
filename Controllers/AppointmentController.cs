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
    [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Staff},{UserRoleType.Constant.Nurse},{UserRoleType.Constant.Doctor}")]
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
        [Authorize]
        public async Task<AppointmentDto> Create(AppointmentDto details)
        {
            var appointment = await AppointmentService.CreateAppointmentRequest(details);
            return DtoMapper.Map(appointment);
        }

        [HttpPost]
        public async Task<AppointmentVisitDto> CreateVisit(AppointmentVisitDto details)
        {
            var visit = await AppointmentService.CreateVisit(details);
            return DtoMapper.Map(visit);
        }

        [HttpPatch]
        public async Task SetApproval(Guid appointmentId, ApprovalStatus approval)
        {
            await AppointmentService.SetAppointmentApproval(appointmentId, approval);
        }

        [HttpPatch]
        public async Task UpdateVisit(AppointmentVisitDto details)
        {
            await AppointmentService.UpdatePatientVisitDetails(details);
        }

        [HttpPatch]
        public async Task SetBlacklist(Guid patientUserId, bool newState)
        {
            await AppointmentService.SetPatientBlacklistState(patientUserId, newState);
        }
    }
}
