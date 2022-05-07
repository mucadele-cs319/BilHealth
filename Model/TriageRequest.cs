using System.ComponentModel.DataAnnotations;
using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model
{
    public class TriageRequest
    {
        [Required] public Guid Id { get; private set; }
        [Required] public Instant DateTime { get; set; }
        [Required] public Guid RequestingUserId { get; set; }
        public DomainUser RequestingUser { get; set; } = null!;
        [Required] public Guid DoctorUserId { get; set; }
        public Doctor DoctorUser { get; set; } = null!;
        [Required] public Guid CaseId { get; set; }
        public Case Case { get; set; } = null!;
        [Required] public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Waiting;
    }
}
