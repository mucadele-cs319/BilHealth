using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services
{
    public interface ICaseService
    {
        Task CreateCase(CaseDto details);
        Task SetCaseState(Guid caseId, CaseState newState);

        Task<CaseMessage> CreateMessage(CaseMessageDto details);
        Task EditMessage(CaseMessageDto details);
        Task RemoveMessage(Guid messageId);

        Task CreateSystemMessage(CaseSystemMessageDto details);

        Task CreatePrescription(PrescriptionDto details);
        Task UpdatePrescription(PrescriptionDto details);
        Task RemovePrescription(Guid prescriptionId);

        Task CreateTriageRequest(TriageRequestDto details);
        Task SetTriageRequestApproval(TriageRequestDto details, ApprovalStatus approval);
    }
}
