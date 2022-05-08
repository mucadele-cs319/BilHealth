using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model.Dto
{
    public class CaseReportDto
    {
        public Guid Id { get; set; }
        public Instant DateTime { get; set; }
        public string Title { get; set; } = null!;
        public Guid PatientUserId { get; set; }
        public Guid? DoctorUserId { get; set; } = null;
        public string? Diagnosis { get; set; } = null!;
        public CaseType Type { get; set; }
        public CaseState State { get; set; }
        public List<Prescription>? Prescriptions { get; set; } = new();
    }
}
