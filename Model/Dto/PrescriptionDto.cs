using NodaTime;

namespace BilHealth.Model.Dto
{
    public class PrescriptionDto
    {
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }
        public Instant DateTime { get; set; }
        public Guid DoctorUserId { get; set; }

        public string Item { get; set; } = null!;
    }
}
