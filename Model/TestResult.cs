using System.ComponentModel.DataAnnotations;
using BilHealth.Utility.Enum;

namespace BilHealth.Model
{
    public class TestResult
    {
        [Required] public Guid Id { get; private set; }
        [Required] public DateTime DateTime { get; set; }
        [Required] public MedicalTestType Type { get; set; }
        [Required] public string Result { get; set; } = null!; // This needs to have a proper way to store results
    }
}
