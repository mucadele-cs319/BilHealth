using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model.Dto
{
    public class SimpleCaseDto
    {
        public Guid Id { get; set; }
        public Instant DateTime { get; set; }
        public Guid PatientUserId { get; set; }
        public SimpleUserDto? SimplePatientUser { get; set; }
        public Guid? DoctorUserId { get; set; } = null;
        public SimpleUserDto? SimpleDoctorUser { get; set; }
        public CaseType Type { get; set; }
        public CaseState State { get; set; }
        public string Title { get; set; } = null!;

        public int MessageCount { get; set; }
    }
}
