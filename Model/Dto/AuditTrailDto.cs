using NodaTime;

namespace BilHealth.Model.Dto
{
    public class AuditTrailDto
    {
        public Guid Id { get; set; }
        public Instant AccessTime { get; set; }

        public Guid AccessedPatientUserId { get; set; }
        public Guid UserId { get; set; }
    }
}
