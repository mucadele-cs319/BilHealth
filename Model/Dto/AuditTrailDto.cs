using NodaTime;

namespace BilHealth.Model.Dto
{
    public class AuditTrailDto
    {
        public Guid Id { get; set; }
        public Instant AccessTime { get; set; }

        public SimpleUserDto AccessedUser { get; set; } = null!;
        public SimpleUserDto AccessingUser { get; set; } = null!;
    }
}
