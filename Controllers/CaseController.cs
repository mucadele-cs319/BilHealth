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
    public class CaseController : ControllerBase
    {
        private readonly IAuthenticationService AuthenticationService;
        private readonly ICaseService CaseService;

        public CaseController(IAuthenticationService authenticationService, ICaseService caseService)
        {
            AuthenticationService = authenticationService;
            CaseService = caseService;
        }

        [HttpPost]
        public async Task<CaseDto> Create(CaseDto details)
        {
            var _case = await CaseService.CreateCase(details);
            return DtoMapper.Map(_case);
        }

        [HttpPatch]
        public async Task<IActionResult> CaseState(Guid caseId, CaseState state)
        {
            await CaseService.SetCaseState(caseId, state);
            return Ok();
        }

        [HttpPost]
        public async Task<CaseMessageDto> CreateMessage(CaseMessageDto details)
        {
            details.UserId = (await AuthenticationService.GetAppUser(User)).DomainUser.Id;
            return DtoMapper.Map(await CaseService.CreateMessage(details));
        }

        [HttpPatch]
        public async Task<CaseMessageDto> EditMessage(CaseMessageDto details)
        {
            return DtoMapper.Map(await CaseService.EditMessage(details));
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveMessage(Guid messageId)
        {
            return await CaseService.RemoveMessage(messageId) ? Ok() : NotFound();
        }

        [HttpPost]
        [Authorize(Roles = UserRoleType.Constant.Doctor)]
        public async Task<PrescriptionDto> CreatePrescription(PrescriptionDto details)
        {
            details.DoctorUserId = (await AuthenticationService.GetAppUser(User)).DomainUser.Id;
            var prescription = await CaseService.CreatePrescription(details);
            return DtoMapper.Map(prescription);
        }

        [HttpPatch]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Doctor}")]
        public async Task<PrescriptionDto> UpdatePrescription(PrescriptionDto details)
        {
            return DtoMapper.Map(await CaseService.UpdatePrescription(details));
        }

        [HttpDelete]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Doctor}")]
        public async Task<IActionResult> RemovePrescription(Guid prescriptionId)
        {
            return await CaseService.RemovePrescription(prescriptionId) ? Ok() : NotFound();
        }

        [HttpPost]
        public async Task<TriageRequestDto> CreateTriageRequest(TriageRequestDto details)
        {
            var triageRequest = await CaseService.CreateTriageRequest(details);
            return DtoMapper.Map(triageRequest);
        }

        [HttpPatch]
        public async Task<IActionResult> TriageRequest(TriageRequestDto details)
        {
            await CaseService.SetTriageRequestApproval(details);
            return Ok();
        }
    }
}
