using System.ComponentModel.DataAnnotations.Schema;
using BilHealth.Utility.Enum;

namespace BilHealth.Model
{
    public class Patient : DomainUser
    {
        public double? BodyWeight { get; set; }
        public double? BodyHeight { get; set; }
        public BloodType BloodType { get; set; } = BloodType.Unspecified;

        public List<Vaccination>? Vaccinations { get; set; }
        public List<TestResult>? TestResults { get; set; }
        [InverseProperty("PatientUser")] public List<TimedAccessGrant>? TimedAccessGrants { get; set; }

        [InverseProperty("PatientUser")] public List<Case>? Cases { get; set; }
        public bool Blacklisted { get; set; } = false;
    }
}
