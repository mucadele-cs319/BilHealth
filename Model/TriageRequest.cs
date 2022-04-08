using System.ComponentModel.DataAnnotations;
using BilHealth.Utility.Enum;

namespace BilHealth.Model
{
    public class TriageRequest
    {
        [Required] public Guid Id { get; private set; }
        [Required] public Guid NurseUserId { get; set; }
        public Nurse NurseUser { get; set; } = null!;
        [Required] public Guid DoctorUserId { get; set; }
        public Doctor DoctorUser { get; set; } = null!;
        [Required] public Guid CaseId { get; set; }
        public Case? Case { get; set; }
        [Required] public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Waiting;
    }
}
