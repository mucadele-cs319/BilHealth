using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace BilHealth.Model
{
    public class AuditTrail
    {
        [Required] public Guid Id { get; private set; }
        [Required] public Instant AccessTime { get; set; }

        [Required] public Guid AccessedUserId { get; set; }
        public Patient AccessedUser { get; set; } = null!;
        [Required] public Guid AccessingUserId { get; set; }
        public DomainUser AccessingUser { get; set; } = null!;
    }
}
