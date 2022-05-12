using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model.Dto
{
    public class TriageRequestDto
    {
        public Guid Id { get; set; }
        public Instant DateTime { get; set; }
        public SimpleUserDto RequestingUser { get; set; } = null!;
        public SimpleUserDto DoctorUser { get; set; } = null!;
        public Guid CaseId { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
