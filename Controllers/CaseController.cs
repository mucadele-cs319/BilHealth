using BilHealth.Model.Dto;
using BilHealth.Model.Dto.Incoming;
using BilHealth.Services;
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
    public class CaseController : ControllerBase
    {
        private readonly IAuthenticationService AuthenticationService;
        private readonly IAccessControlService AccessControlService;
        private readonly ICaseService CaseService;

        public CaseController(IAuthenticationService authenticationService, IAccessControlService accessControlService, ICaseService caseService)
        {
            AuthenticationService = authenticationService;
            AccessControlService = accessControlService;
            CaseService = caseService;
        }

        [HttpGet]
        public async Task<List<SimpleCaseDto>> GetPersonalized()
        {
            var user = await AuthenticationService.GetUser(User);
            var list = await AccessControlService.GetPersonalizedCaseList(user);
            return list.Select(DtoMapper.MapSimpleCase).ToList();
        }

        [HttpGet("{caseId:guid}")]
        public async Task<IActionResult> Get(Guid caseId)
        {
            var user = await AuthenticationService.GetUser(User);
            try
            {
                if (!(await AccessControlService.Case(user.Id, caseId)))
                    return Forbid();
            }
            catch (IdNotFoundException) { return NotFound(); }

            return Ok(DtoMapper.Map(await CaseService.GetCase(caseId)));
        }

        [HttpPost]
        public async Task<CaseDto> Create(CaseCreateDto details)
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
        public async Task<CaseMessageDto> CreateMessage(Guid caseId, CaseMessageUpdateDto details)
        {
            var userId = (await AuthenticationService.GetUser(User)).Id;
            return DtoMapper.Map(await CaseService.CreateMessage(caseId, userId, details));
        }

        [HttpPut("messages/{messageId:guid}")]
        public async Task<CaseMessageDto> UpdateMessage(Guid messageId, CaseMessageUpdateDto details)
        {
            return DtoMapper.Map(await CaseService.EditMessage(messageId, details));
        }

        [HttpDelete("messages/{messageId:guid}")]
        public async Task<IActionResult> DeleteMessage(Guid messageId)
        {
            return await CaseService.RemoveMessage(messageId) ? Ok() : NotFound();
        }

        [HttpPost("{caseId:guid}/prescriptions")]
        [Authorize(Roles = UserType.Doctor)]
        public async Task<PrescriptionDto> CreatePrescription(Guid caseId, PrescriptionUpdateDto details)
        {
            var doctorUserId = (await AuthenticationService.GetUser(User)).Id;
            var prescription = await CaseService.CreatePrescription(caseId, doctorUserId, details);
            return DtoMapper.Map(prescription);
        }

        [HttpPut("prescriptions/{prescriptionId:guid}")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Doctor}")]
        public async Task<PrescriptionDto> UpdatePrescription(Guid prescriptionId, PrescriptionUpdateDto details)
        {
            return DtoMapper.Map(await CaseService.UpdatePrescription(prescriptionId, details));
        }

        [HttpDelete("prescriptions/{prescriptionId:guid}")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Doctor}")]
        public async Task<IActionResult> RemovePrescription(Guid prescriptionId)
        {
            return await CaseService.RemovePrescription(prescriptionId) ? Ok() : NotFound();
        }

        [HttpPost("{caseId:guid}/triagerequest")]
        [Authorize(Roles = $"{UserType.Nurse},{UserType.Patient}")]
        public async Task<TriageRequestDto> CreateTriageRequest(Guid caseId, [FromQuery] Guid doctorUserId)
        {
            var requestingUserId = (await AuthenticationService.GetUser(User)).Id;
            var triageRequest = await CaseService.CreateTriageRequest(caseId, requestingUserId, doctorUserId);
            return DtoMapper.Map(triageRequest);
        }

        [HttpPatch("{caseId:guid}/triagerequest")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Doctor},{UserType.Staff}")]
        public async Task<IActionResult> SetTriageRequestApproval(Guid caseId, ApprovalStatus approval)
        {
            await CaseService.SetTriageRequestApproval(caseId, approval);
            return Ok();
        }

        [HttpPatch("{caseId:guid}/diagnosis")]
        [Authorize(Roles = UserType.Doctor)]
        public async Task<CaseDto> SetDiagnosis(Guid caseId, [FromBody] string diagnosis)
        {

            return DtoMapper.Map(await CaseService.SetDiagnosis(caseId, diagnosis));
        }

        [HttpPatch("{caseId:guid}/unassign")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Doctor},{UserType.Staff}")]
        public async Task<CaseDto> DeleteDoctor(Guid caseId)
        {
            return DtoMapper.Map(await CaseService.UnassignDoctor(caseId));
        }

        [HttpPatch("{caseId:guid}/report")]
        [Authorize(Roles = $"{UserType.Patient},{UserType.Doctor},{UserType.Staff}")]
        public async Task<CaseDto> CreateReport(Guid patientId)
        {
            return DtoMapper.Map(await CaseService.CreateReport(patientId));
        }
    }
}
