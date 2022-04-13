using System.ComponentModel.DataAnnotations;
using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model
{
    public class Appointment
    {
        [Required] public Guid Id { get; private set; }
        [Required] public Instant CreatedAt { get; set; }
        [Required] public Instant DateTime { get; set; }
        [Required] public string Description { get; set; } = null!;
        [Required] public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Waiting;
        [Required] public bool Attended { get; set; } = false;

        [Required] public Guid CaseId { get; set; }
        public Case? Case { get; set; }

        public AppointmentVisit? Visit { get; set; }
    }
}
