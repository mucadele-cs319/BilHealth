using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto;
using NodaTime;

namespace BilHealth.Services
{
    public class TestResultService : DbServiceBase, ITestResultService
    {
        private readonly IClock Clock;
        private readonly string fileStorePath = Path.Combine("/", "testresults");

        public TestResultService(AppDbContext dbCtx, IClock clock) : base(dbCtx)
        {
            Clock = clock;
        }

        private async Task<string> SaveFile(IFormFile file, string? fileName = null)
        {
            var filePath = Path.Combine(fileStorePath, fileName is null ? Guid.NewGuid().ToString() + Path.GetExtension(file.FileName) : fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            return filePath;
        }

        public async Task<TestResult> CreateTestResult(TestResultDto details, IFormFile? testResultFile)
        {
            var testResult = new TestResult
            {
                DateTime = Clock.GetCurrentInstant(),
                PatientUserId = details.PatientUserId,
                Type = details.Type
            };
            DbCtx.TestResults.Add(testResult);

            if (testResultFile is not null && testResultFile.Length > 0)
            {
                testResult.FileName = await SaveFile(testResultFile);
            }

            await DbCtx.SaveChangesAsync();
            return testResult;
        }

        public async Task<bool> RemoveTestResult(Guid testResultId)
        {
            var testResult = await DbCtx.TestResults.FindAsync(testResultId);
            if (testResult is null) return false;

            DbCtx.TestResults.Remove(testResult);
            if (testResult.FileName is not null)
                File.Delete(Path.Combine(fileStorePath, testResult.FileName));
            await DbCtx.SaveChangesAsync();
            return true;
        }

        public async Task<TestResult> UpdateTestResult(TestResultDto details, IFormFile? testResultFile)
        {
            var testResult = await DbCtx.TestResults.FindAsync(details.Id);
            if (testResult is null) throw new ArgumentException("No test result with ID " + details.Id);

            testResult.Type = details.Type;

            if (testResultFile is not null && testResultFile.Length > 0)
            {
                testResult.FileName = await SaveFile(testResultFile, testResult.FileName);
            }

            await DbCtx.SaveChangesAsync();
            return testResult;
        }

        public async Task<FileStream> GetTestResultFile(Guid testResultId)
        {
            var testResult = await DbCtx.TestResults.FindAsync(testResultId);
            if (testResult is null) throw new ArgumentException("No test result with ID " + testResultId);
            if (testResult.FileName is null) throw new Exception("This test result does not have a file");

            return new FileStream(Path.Combine(fileStorePath, testResult.FileName), FileMode.Open);
        }
    }
}
