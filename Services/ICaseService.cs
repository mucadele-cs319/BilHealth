using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services
{
    public interface ICaseService
    {
        Task<Case> CreateCase(CaseDto details);
        Task SetCaseState(Guid caseId, CaseState newState);

        Task<CaseMessage> CreateMessage(CaseMessageDto details);
        Task<CaseMessage> EditMessage(CaseMessageDto details);
        Task<bool> RemoveMessage(Guid messageId);

        void CreateSystemMessage(Guid caseId, CaseSystemMessageType type, string content);

        Task<Prescription> CreatePrescription(PrescriptionDto details);
        Task<Prescription> UpdatePrescription(PrescriptionDto details);
        Task<bool> RemovePrescription(Guid prescriptionId);

        Task<TriageRequest> CreateTriageRequest(TriageRequestDto details);
        Task SetTriageRequestApproval(TriageRequestDto details);
    }
}
