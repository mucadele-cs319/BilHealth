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

        [HttpPost]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Staff}")]
        public async Task<TestResultDto> Create([FromForm] TestResultDto details, IFormFile? file)
        {
            var testResult = await TestResultService.CreateTestResult(details, file);
            return DtoMapper.Map(testResult);
        }

        [HttpGet("{testResultId:guid}/file")]
        [Produces("application/pdf")]
        public async Task<IActionResult> GetFile(Guid testResultId)
        {
            var fileStream = await TestResultService.GetTestResultFile(testResultId);
            return File(fileStream, "application/pdf");
        }

        [HttpPatch("{testResultId:guid}")]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Staff}")]
        public async Task<IActionResult> Update(Guid testResultId, [FromForm] TestResultDto details, IFormFile? file)
        {
            details.Id = testResultId;
            return Ok(DtoMapper.Map(await TestResultService.UpdateTestResult(details, file)));
        }

        [HttpDelete("{testResultId:guid}")]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Staff}")]
        public async Task<IActionResult> Delete(Guid testResultId)
        {
            return await TestResultService.RemoveTestResult(testResultId) ? Ok() : NotFound();
        }
    }
}
