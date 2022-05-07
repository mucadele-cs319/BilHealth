using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model.Dto.Incoming
{
    public class UserProfileUpdateDto
    {
        // User Fields
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public LocalDate? DateOfBirth { get; set; }

        // Patient Fields
        public double? BodyWeight { get; set; }
        public double? BodyHeight { get; set; }
        public BloodType? BloodType { get; set; }

        // Doctor Fields
        public string? Specialization { get; set; }
        public Campus? Campus { get; set; }
    }
}
