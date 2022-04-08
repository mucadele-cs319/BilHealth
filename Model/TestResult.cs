using System.ComponentModel.DataAnnotations;
using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model
{
    public class TestResult
    {
        [Required] public Guid Id { get; private set; }
        [Required] public Guid PatientUserId { get; set; }
        public Patient PatientUser { get; set; } = null!;

        [Required] public Instant DateTime { get; set; }
        [Required] public MedicalTestType Type { get; set; }
        [Required] public string ResultFilePath { get; set; } = null!;
    }
}
