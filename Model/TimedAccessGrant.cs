using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace BilHealth.Model
{
    public class TimedAccessGrant
    {
        [Required] public Guid Id { get; private set; }
        [Required] public Instant ExpiryTime { get; set; }
        [Required] public Duration Duration { get; set; }
        [Required] public bool Canceled { get; set; } = false;

        [Required] public Guid PatientUserId { get; set; }
        public Patient PatientUser { get; set; } = null!;
        [Required] public Guid UserId { get; set; }
        public DomainUser User { get; set; } = null!;
    }
}
