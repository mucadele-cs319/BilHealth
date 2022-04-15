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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetFile(Guid id)
        {
            var fileStream = await TestResultService.GetTestResultFile(id);
            return File(fileStream, "application/pdf");
        }

        [HttpPatch]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Staff}")]
        public async Task<IActionResult> Update([FromForm] TestResultDto details, IFormFile? file)
        {
            if (details.Id is null) return BadRequest();
            return Ok(DtoMapper.Map(await TestResultService.UpdateTestResult(details, file)));
        }

        [HttpDelete]
        [Authorize(Roles = $"{UserRoleType.Constant.Admin},{UserRoleType.Constant.Staff}")]
        public async Task<IActionResult> Delete(Guid testResultId)
        {
            return await TestResultService.RemoveTestResult(testResultId) ? Ok() : NotFound();
        }
    }
}
