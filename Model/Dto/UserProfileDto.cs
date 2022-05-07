using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model.Dto
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string UserType { get; set; } = null!;

        // User Fields
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public Gender Gender { get; set; } = Gender.Unspecified;
        public LocalDate? DateOfBirth { get; set; }

        // Patient Fields
        public double? BodyWeight { get; set; }
        public double? BodyHeight { get; set; }
        public BloodType BloodType { get; set; } = BloodType.Unspecified;

        public List<VaccinationDto>? Vaccinations { get; set; }
        public List<TestResultDto>? TestResults { get; set; }
        public List<TimedAccessGrantDto>? TimedAccessGrants { get; set; }

        public List<SimpleCaseDto>? Cases { get; set; }
        public bool? Blacklisted { get; set; }

        // Doctor Fields
        public string? Specialization { get; set; }
        public Campus? Campus { get; set; }
    }
}
