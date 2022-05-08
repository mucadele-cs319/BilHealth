using BilHealth.Model;
using BilHealth.Model.Dto.Incoming;
using BilHealth.Utility.Enum;

namespace BilHealth.Services
{
    public interface ICaseService
    {
        Task<Case> GetCase(Guid caseId);
        Task<Case> CreateCase(CaseCreateDto details);
        Task SetCaseState(Guid caseId, CaseState newState);

        Task<CaseMessage> CreateMessage(Guid caseId, Guid userId, CaseMessageUpdateDto details);
        Task<CaseMessage> EditMessage(Guid messageId, CaseMessageUpdateDto details);
        Task<bool> RemoveMessage(Guid messageId);

        void CreateSystemMessage(Guid caseId, CaseSystemMessageType type, string content);

        Task<Prescription> CreatePrescription(Guid caseId, Guid doctorUserId, PrescriptionUpdateDto details);
        Task<Prescription> UpdatePrescription(Guid prescriptionId, PrescriptionUpdateDto details);
        Task<bool> RemovePrescription(Guid prescriptionId);

        Task<TriageRequest> CreateTriageRequest(Guid caseId, Guid requestingUserId, Guid doctorUserId);
        Task SetTriageRequestApproval(Guid caseId, ApprovalStatus approval);

        Task<Case> SetDiagnosis(Guid caseId, string diagnosis);
        Task<Case> UnassignDoctor(Guid caseId);

        Task<Case> CreateReport(Guid patientId);
    }
}
