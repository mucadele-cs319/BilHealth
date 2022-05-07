using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Services.Users;
using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Services
{
    public class TestResultService : DbServiceBase, ITestResultService
    {
        private readonly INotificationService NotificationService;
        private readonly IClock Clock;
        private readonly string fileStorePath = Path.Combine("/", "testresults");

        public TestResultService(AppDbContext dbCtx, INotificationService notificationService, IClock clock) : base(dbCtx)
        {
            NotificationService = notificationService;
            Clock = clock;
        }

        private async Task<string> SaveFile(IFormFile file, string? fileName = null)
        {
            var actualFileName = fileName is null ? Guid.NewGuid().ToString() + Path.GetExtension(file.FileName) : fileName;
            var filePath = Path.Combine(fileStorePath, actualFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            return actualFileName;
        }

        private void DeleteFile(TestResult testResult)
        {
            if (testResult.FileName is null) return;

            var filePath = Path.Combine(fileStorePath, testResult.FileName);
            File.Delete(filePath);
        }

        public async Task<TestResult> CreateTestResult(Guid patientUserId, MedicalTestType testType, IFormFile? testResultFile)
        {
            var testResult = new TestResult
            {
                DateTime = Clock.GetCurrentInstant(),
                PatientUserId = patientUserId,
                Type = testType
            };
            DbCtx.TestResults.Add(testResult);

            if (testResultFile is not null && testResultFile.Length > 0)
            {
                testResult.FileName = await SaveFile(testResultFile);
            }

            NotificationService.AddNewTestResultNotification(patientUserId, testResult);

            try
            {
                await DbCtx.SaveChangesAsync();
            }
            catch (Exception)
            {
                DeleteFile(testResult);
                throw;
            }

            return testResult;
        }

        public async Task<bool> RemoveTestResult(Guid testResultId)
        {
            var testResult = await DbCtx.TestResults.FindAsync(testResultId);
            if (testResult is null) return false;

            DeleteFile(testResult);
            DbCtx.TestResults.Remove(testResult);
            await DbCtx.SaveChangesAsync();
            return true;
        }

        public async Task<TestResult> UpdateTestResult(Guid testResultId, MedicalTestType testType, IFormFile? testResultFile)
        {
            var testResult = await DbCtx.TestResults.FindOrThrowAsync(testResultId);

            testResult.Type = testType;

            if (testResultFile is not null && testResultFile.Length > 0)
            {
                testResult.FileName = await SaveFile(testResultFile, testResult.FileName);
            }

            await DbCtx.SaveChangesAsync();
            return testResult;
        }

        public async Task<FileStream> GetTestResultFile(Guid testResultId)
        {
            var testResult = await DbCtx.TestResults.FindOrThrowAsync(testResultId);
            if (testResult.FileName is null) throw new InvalidOperationException($"Test result ({testResultId}) does not have a file");

            return new FileStream(Path.Combine(fileStorePath, testResult.FileName), FileMode.Open);
        }
    }
}
