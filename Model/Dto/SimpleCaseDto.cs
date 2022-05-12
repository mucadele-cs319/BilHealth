using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model.Dto
{
    public class SimpleCaseDto
    {
        public Guid Id { get; set; }
        public Instant DateTime { get; set; }
        public SimpleUserDto PatientUser { get; set; } = null!;
        public SimpleUserDto? DoctorUser { get; set; }
        public CaseType Type { get; set; }
        public CaseState State { get; set; }
        public string Title { get; set; } = null!;

        public int MessageCount { get; set; }
    }
}
