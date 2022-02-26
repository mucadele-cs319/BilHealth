using BilHealth.Utility.Enum;
using System.ComponentModel.DataAnnotations;

namespace BilHealth.Model
{
    public class PatientInfo
    {
        [Required] public Guid Id { get; private set; }
        public double? BodyWeight { get; set; }
        public double? BodyHeight { get; set; }
        public BloodType? BloodType { get; set; }
        public List<TestResult>? TestResults { get; set; }
        public List<Case>? Cases { get; set; }
    }
}
