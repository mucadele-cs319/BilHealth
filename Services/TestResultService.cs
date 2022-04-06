using System.Security.Claims;
using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;
using Microsoft.AspNetCore.Identity;

namespace BilHealth.Services
{
    public class TestResultService : DbServiceBase, ITestResultService
    {
        public TestResultService(AppDbContext dbCtx) : base(dbCtx)
        {
        }

        public Task CreateTestResult(TestResultDto details, IFormFile? testResultFile)
        {
            throw new NotImplementedException();
        }

        public Task RemoveTestResult(Guid testResultId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTestResult(TestResultDto details, IFormFile? testResultFile)
        {
            throw new NotImplementedException();
        }
    }
}
