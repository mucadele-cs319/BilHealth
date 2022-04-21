using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model.Dto
{
    public record SimpleCaseDto
    {
        public Guid Id { get; set; }
        public Instant DateTime { get; set; }
        public Guid PatientUserId { get; set; }
        public Guid? DoctorUserId { get; set; } = null;
        public CaseState State { get; set; }

        public int MessageCount { get; set; }
    }
}
