using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services
{
    public interface ICaseService
    {
        Task CreateCase(CaseDto details);
        Task ReopenCase(Guid caseId);
        Task CloseCase(Guid caseId);
        Task SetCaseState(Guid caseId, CaseState newState);

        Task CreateMessage(CaseMessageDto details);
        Task CreateSystemMessage(CaseSystemMessageDto details);

        Task CreatePrescription(PrescriptionDto details);
        Task UpdatePrescription(PrescriptionDto details);

        Task CreateTriageRequest(TriageRequestDto details);
        Task SetTriageRequestApproval(TriageRequestDto details, ApprovalStatus approval);
    }
}
