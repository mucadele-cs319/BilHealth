using System.ComponentModel.DataAnnotations;
using BilHealth.Utility.Enum;

namespace BilHealth.Model
{
    public class Appointment
    {
        [Required] public Guid Id { get; private set; }
        [Required] public DateTime CreatedAt { get; set; }
        [Required] public DateTime DateTime { get; set; }
        [Required] public string Description { get; set; } = null!;
        [Required] public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Waiting;
        [Required] public bool Attended { get; set; } = false;

        [Required] public Guid CaseId { get; set; }
        public Case? Case { get; set; }

        public List<AppointmentVisit>? Visits { get; set; }
    }
}