using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services
{
    public interface ITestResultService
    {
        Task CreateTestResult(TestResultDto details, IFormFile? testResultFile);
        Task UpdateTestResult(TestResultDto details, IFormFile? testResultFile);
        Task RemoveTestResult(Guid testResultId);
    }
}
