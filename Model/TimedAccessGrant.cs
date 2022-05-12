using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace BilHealth.Model
{
    public class TimedAccessGrant
    {
        [Required] public Guid Id { get; private set; }
        [Required] public Instant ExpiryTime { get; set; }
        [Required] public Period Period { get; set; } = null!;
        [Required] public bool Canceled { get; set; } = false;

        [Required] public Guid PatientUserId { get; set; }
        public Patient PatientUser { get; set; } = null!;
        [Required] public Guid GrantedUserId { get; set; }
        public DomainUser GrantedUser { get; set; } = null!;
    }
}
