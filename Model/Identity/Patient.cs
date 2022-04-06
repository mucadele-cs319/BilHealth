using BilHealth.Utility.Enum;

namespace BilHealth.Model
{
    public class Patient : User
    {
        public double? BodyWeight { get; set; }
        public double? BodyHeight { get; set; }
        public BloodType? BloodType { get; set; }

        public List<Vaccination>? Vaccinations { get; set; }
        public List<TestResult>? TestResults { get; set; }

        public List<Case>? Cases { get; set; }
        public bool Blacklisted { get; set; } = false;
    }
}
