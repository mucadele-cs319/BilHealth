using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model.Dto
{
    public record TestResultDto
    {
        public Guid? Id { get; set; }
        public Guid PatientUserId { get; set; }

        public Instant DateTime { get; set; }
        public MedicalTestType Type { get; set; }
    }
}
