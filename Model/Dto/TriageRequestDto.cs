using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model.Dto
{
    public class TriageRequestDto
    {
        public Guid Id { get; set; }
        public Instant DateTime { get; set; }
        public Guid RequestingUserId { get; set; }
        public Guid DoctorUserId { get; set; }
        public Guid CaseId { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
