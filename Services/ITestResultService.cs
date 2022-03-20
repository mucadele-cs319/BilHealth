using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services
{
    public interface ITestResultService
    {
        public Task CreateTestResult();
        public Task UpdateTestResult();
    }
}
