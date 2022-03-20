using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services
{
    public interface ICaseService
    {
        public Task CreateCase();
        public Task ReopenCase();
        public Task CloseCase();
        public Task SetCaseState();

        public Task CreateMessage();
        public Task CreateSystemMessage();

        public Task CreatePrescription();
        public Task UpdatePrescription();

        public Task CreateTriageRequest();
        public Task ApproveTriageRequest();
        public Task RejectTriageRequest();
    }
}
