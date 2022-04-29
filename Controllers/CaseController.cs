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
    public class CaseController : ControllerBase
    {
        private readonly IAuthenticationService AuthenticationService;
        private readonly ICaseService CaseService;

        public CaseController(IAuthenticationService authenticationService, ICaseService caseService)
        {
            AuthenticationService = authenticationService;
            CaseService = caseService;
        }

        [HttpGet("{caseId:guid}")]
        public async Task<IActionResult> Get(Guid caseId)
        {
            var user = await AuthenticationService.GetAppUser(User);
            if (!(await AuthenticationService.CanAccessCase(user.DomainUser, caseId)))
                return Forbid();

            return Ok(DtoMapper.Map(await CaseService.GetCase(caseId)));
        }

        [HttpPost]
        public async Task<CaseDto> Create(CaseDto details)
        {
            var _case = await CaseService.CreateCase(details);
            return DtoMapper.Map(_case);
        }

        [HttpPatch("{caseId:guid}")]
        public async Task<IActionResult> CaseState(Guid caseId, CaseState state)
        {
            await CaseService.SetCaseState(caseId, state);
            return Ok();
        }

        [HttpPost("{caseId:guid}/messages")]
        public async Task<CaseMessageDto> CreateMessage(Guid caseId, CaseMessageDto details)
        {
            details.CaseId = caseId;
            details.UserId = (await AuthenticationService.GetAppUser(User)).DomainUser.Id;
            return DtoMapper.Map(await CaseService.CreateMessage(details));
        }

        [HttpPut("messages/{messageId:guid}")]
        public async Task<CaseMessageDto> UpdateMessage(Guid messageId, CaseMessageDto details)
        {
            details.Id = messageId;
            return DtoMapper.Map(await CaseService.EditMessage(details));
        }

        [HttpDelete("messages/{messageId:guid}")]
        public async Task<IActionResult> DeleteMessage(Guid messageId)
        {
            return await CaseService.RemoveMessage(messageId) ? Ok() : NotFound();
        }

        [HttpPost("{caseId:guid}/prescriptions")]
        [Authorize(Roles = UserRoleType.Constant.Doctor)]
        public async Task<PrescriptionDto> CreatePrescription(Guid caseId, PrescriptionDto details)
        {
            details.CaseId = caseId;
            details.DoctorUserId = (await AuthenticationService.GetAppUser(User)).DomainUser.Id;
            var prescription = await CaseService.CreatePrescription(details);
            return DtoMapper.Map(prescription);
        }

        [HttpPut("prescriptions/{prescriptionId:guid}")]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Doctor}")]
        public async Task<PrescriptionDto> UpdatePrescription(Guid prescriptionId, PrescriptionDto details)
        {
            details.Id = prescriptionId;
            return DtoMapper.Map(await CaseService.UpdatePrescription(details));
        }

        [HttpDelete("prescriptions/{prescriptionId:guid}")]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Doctor}")]
        public async Task<IActionResult> RemovePrescription(Guid prescriptionId)
        {
            return await CaseService.RemovePrescription(prescriptionId) ? Ok() : NotFound();
        }

        [HttpPost("{caseId:guid}/triagerequest")]
        [Authorize(Roles = UserRoleType.Constant.Nurse)]
        public async Task<TriageRequestDto> CreateTriageRequest(Guid caseId, TriageRequestDto details)
        {
            details.CaseId = caseId;
            details.NurseUserId = (await AuthenticationService.GetAppUser(User)).DomainUser.Id;
            var triageRequest = await CaseService.CreateTriageRequest(details);
            return DtoMapper.Map(triageRequest);
        }

        [HttpPatch("{caseId:guid}/triagerequest")]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Doctor},{UserRoleType.Constant.Staff}")]
        public async Task<IActionResult> SetTriageRequestApproval(Guid caseId, TriageRequestDto details)
        {
            details.CaseId = caseId;
            await CaseService.SetTriageRequestApproval(details);
            return Ok();
        }

        [HttpPatch("{caseId:guid}/diagnosis")]
        [Authorize(Roles = UserRoleType.Constant.Doctor)]
        public async Task<CaseDto> SetDiagnosis(Guid caseId, string diagnosis)
        {

            return DtoMapper.Map(await CaseService.SetDiagnosis(caseId, diagnosis));
        }
    }
}
