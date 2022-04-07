using BilHealth.Utility.Enum;

namespace BilHealth.Model.Dto
{
    public record UserProfileDto
    {
        public string UserType { get; set; } = null!;

        // User Fields
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender Gender { get; set; } = Gender.Unspecified;
        public DateTime? DateOfBirth { get; set; }

        // Patient Fields
        public double? BodyWeight { get; set; }
        public double? BodyHeight { get; set; }
        public BloodType? BloodType { get; set; }

        public List<Vaccination>? Vaccinations { get; set; }
        public List<TestResult>? TestResults { get; set; }

        public List<Case>? Cases { get; set; }
        public bool Blacklisted { get; set; } = false;

        // Nurse Fields
        public List<TriageRequest>? TriageRequests { get; set; }

        // Doctor Fields
        public string Specialization { get; set; } = String.Empty;
        public Campus Campus { get; set; }
    }
}
