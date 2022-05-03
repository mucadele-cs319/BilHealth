using BilHealth.Utility.Enum;

namespace BilHealth.Model.Dto
{
    public record TriageRequestDto
    {
        public Guid Id { get; set; }
        public Guid RequestingUserId { get; set; }
        public Guid NurseUserId { get; set; }
        public Guid DoctorUserId { get; set; }
        public Guid CaseId { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
