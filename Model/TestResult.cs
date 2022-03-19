using System.ComponentModel.DataAnnotations;
using BilHealth.Utility.Enum;

namespace BilHealth.Model
{
    public class TestResult
    {
        [Required] public Guid Id { get; private set; }
        [Required] public Guid PatientUserId { get; set; }
        public User PatientUser { get; set; } = null!;

        [Required] public DateTime DateTime { get; set; }
        [Required] public MedicalTestType Type { get; set; }
        [Required] public string ResultFilePath { get; set; } = null!;
    }
}
