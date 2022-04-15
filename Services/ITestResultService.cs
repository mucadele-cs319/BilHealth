using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services
{
    public interface ITestResultService
    {
        Task<TestResult> CreateTestResult(TestResultDto details, IFormFile? testResultFile);
        Task<TestResult> UpdateTestResult(TestResultDto details, IFormFile? testResultFile);
        Task<bool> RemoveTestResult(Guid testResultId);

        Task<FileStream> GetTestResultFile(Guid testResultId);
    }
}
