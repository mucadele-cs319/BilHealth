using BilHealth.Model.Dto;
using BilHealth.Model.Dto.Incoming;
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

        [HttpPost("/api/cases/{caseId:guid}/appointment")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Staff},{UserType.Patient},{UserType.Doctor}")]
        public async Task<IActionResult> Create(Guid caseId, AppointmentUpdateDto details)
        {
            var requestingUserId = (await AuthenticationService.GetUser(User)).Id;
            try
            {
                var appointment = await AppointmentService.CreateAppointment(caseId, requestingUserId, details);
                return Ok(DtoMapper.Map(appointment));
            }
            catch (InvalidOperationException)
            {
                return Forbid("You are probably blacklisted.");
            }
        }

        [HttpPatch("{appointmentId:guid}")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Staff},{UserType.Doctor}")]
        public async Task<AppointmentDto> Update(Guid appointmentId, AppointmentUpdateDto details)
        {
            var appointment = await AppointmentService.UpdateAppointment(appointmentId, details);
            return DtoMapper.Map(appointment);
        }

        [HttpPut("{appointmentId:guid}/cancel")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Staff},{UserType.Patient},{UserType.Doctor}")]
        public async Task<IActionResult> Cancel(Guid appointmentId)
        {
            var success = await AppointmentService.CancelAppointment(appointmentId);
            return success ? Ok() : NotFound();
        }

        [HttpPut("{appointmentId:guid}/approval")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Staff},{UserType.Doctor}")]
        public async Task SetApproval(Guid appointmentId, ApprovalStatus approval)
        {
            await AppointmentService.SetAppointmentApproval(appointmentId, approval);
        }

        [HttpPost("{appointmentId:guid}/visit")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Nurse},{UserType.Doctor}")]
        public async Task<AppointmentVisitDto> CreateVisit(Guid appointmentId, AppointmentVisitUpdateDto details)
        {
            var visit = await AppointmentService.CreateVisit(appointmentId, details);
            return DtoMapper.Map(visit);
        }

        [HttpPut("{appointmentId:guid}/visit")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Nurse},{UserType.Doctor}")]
        public async Task UpdateVisit(Guid appointmentId, AppointmentVisitUpdateDto details)
        {
            await AppointmentService.UpdatePatientVisitDetails(appointmentId, details);
        }
    }
}
