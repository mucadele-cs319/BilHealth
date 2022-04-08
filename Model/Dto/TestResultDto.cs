using BilHealth.Utility.Enum;

namespace BilHealth.Model.Dto
{
    public record TestResultDto
    {
        public Guid Id { get; set; }
        public Guid PatientUserId { get; set; }

        public DateTime DateTime { get; set; }
        public MedicalTestType Type { get; set; }
    }
}
