using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model.Dto
{
    public class CaseReportDto
    {
        public Instant DateTime { get; set; }
        public string Title { get; set; } = null!;
        public string? Diagnosis { get; set; } = null!;
        public CaseType Type { get; set; }
        public List<Prescription>? Prescriptions { get; set; } = new();
    }
}
