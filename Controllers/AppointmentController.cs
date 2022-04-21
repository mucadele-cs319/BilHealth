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
    [Route("api/[controller]s")]
    [Authorize]
    [Produces("application/json")]
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
            var appointment = await AppointmentService.CreateAppointment(details);
            return DtoMapper.Map(appointment);
        }

        [HttpPut("{appointmentId:guid}/approval")]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Staff},{UserRoleType.Constant.Nurse},{UserRoleType.Constant.Doctor}")]
        public async Task SetApproval(Guid appointmentId, ApprovalStatus approval)
        {
            await AppointmentService.SetAppointmentApproval(appointmentId, approval);
        }

        [HttpPost("{appointmentId:guid}/visit")]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Nurse},{UserRoleType.Constant.Doctor}")]
        public async Task<AppointmentVisitDto> CreateVisit(Guid appointmentId, AppointmentVisitDto details)
        {
            details.AppointmentId = appointmentId;
            var visit = await AppointmentService.CreateVisit(details);
            return DtoMapper.Map(visit);
        }

        [HttpPut("{appointmentId:guid}/visit")]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Staff},{UserRoleType.Constant.Nurse},{UserRoleType.Constant.Doctor}")]
        public async Task UpdateVisit(Guid appointmentId, AppointmentVisitDto details)
        {
            details.AppointmentId = appointmentId;
            await AppointmentService.UpdatePatientVisitDetails(details);
        }
    }
}
