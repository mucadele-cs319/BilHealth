using NodaTime;

namespace BilHealth.Model.Dto
{
    public class PrescriptionDto
    {
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }
        public Instant DateTime { get; set; }
        public SimpleUserDto DoctorUser { get; set; } = null!;

        public string Item { get; set; } = null!;
    }
}
