using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace BilHealth.Model
{
    public class AuditTrail
    {
        [Required] public Guid Id { get; private set; }
        [Required] public Instant AccessTime { get; set; }

        [Required] public Guid AccessedPatientUserId { get; set; }
        public Patient AccessedPatientUser { get; set; } = null!;
        [Required] public Guid UserId { get; set; }
        public DomainUser User { get; set; } = null!;
    }
}
