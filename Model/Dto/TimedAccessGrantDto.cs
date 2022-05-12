using NodaTime;

namespace BilHealth.Model.Dto
{
    public class TimedAccessGrantDto
    {
        public Guid Id { get; set; }
        public Instant ExpiryTime { get; set; }
        public Period Period { get; set; } = null!;
        public bool Canceled { get; set; } = false;

        public Guid PatientUserId { get; set; }
        public SimpleUserDto GrantedUser { get; set; } = null!;
    }
}
