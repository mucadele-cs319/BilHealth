using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model.Dto
{
    public record UserProfileDto
    {
        public Guid? Id { get; set; }
        public string? UserType { get; set; }

        // User Fields
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public LocalDate? DateOfBirth { get; set; }

        // Patient Fields
        public double? BodyWeight { get; set; }
        public double? BodyHeight { get; set; }
        public BloodType? BloodType { get; set; }

        public List<VaccinationDto>? Vaccinations { get; set; }
        public List<TestResultDto>? TestResults { get; set; }

        public List<CaseDto>? Cases { get; set; }
        public bool? Blacklisted { get; set; }

        // Nurse Fields
        public List<TriageRequestDto>? TriageRequests { get; set; }

        // Doctor Fields
        public string? Specialization { get; set; }
        public Campus? Campus { get; set; }
    }
}
