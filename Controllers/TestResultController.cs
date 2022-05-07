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
    public class TestResultController : ControllerBase
    {
        private readonly IAuthenticationService AuthenticationService;
        private readonly ITestResultService TestResultService;

        public TestResultController(IAuthenticationService authenticationService, ITestResultService testResultService)
        {
            AuthenticationService = authenticationService;
            TestResultService = testResultService;
        }

        [HttpPost("/api/profiles/{patientUserId:guid}/testresults")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Staff}")]
        public async Task<TestResultDto> Create(Guid patientUserId, [FromQuery] MedicalTestType testType, IFormFile? file)
        {
            var testResult = await TestResultService.CreateTestResult(patientUserId, testType, file);
            return DtoMapper.Map(testResult);
        }

        [HttpGet("{testResultId:guid}/file")]
        [Produces("application/pdf")]
        public async Task<IActionResult> GetFile(Guid testResultId)
        {
            var user = await AuthenticationService.GetUser(User);
            if (!(await AuthenticationService.CanAccessTestResult(user, testResultId)))
                return Forbid();

            var fileStream = await TestResultService.GetTestResultFile(testResultId);
            return File(fileStream, "application/pdf");
        }

        [HttpPatch("{testResultId:guid}")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Staff}")]
        public async Task<IActionResult> Update(Guid testResultId, [FromQuery] MedicalTestType testType, IFormFile? file)
        {
            return Ok(DtoMapper.Map(await TestResultService.UpdateTestResult(testResultId, testType, file)));
        }

        [HttpDelete("{testResultId:guid}")]
        [Authorize(Roles = $"{UserType.Admin},{UserType.Staff}")]
        public async Task<IActionResult> Delete(Guid testResultId)
        {
            return await TestResultService.RemoveTestResult(testResultId) ? Ok() : NotFound();
        }
    }
}
