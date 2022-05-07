using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services
{
    public interface ITestResultService
    {
        Task<TestResult> CreateTestResult(Guid patientUserId, MedicalTestType testType, IFormFile? testResultFile);
        Task<TestResult> UpdateTestResult(Guid testResultId, MedicalTestType testType, IFormFile? testResultFile);
        Task<bool> RemoveTestResult(Guid testResultId);

        Task<FileStream> GetTestResultFile(Guid testResultId);
    }
}
