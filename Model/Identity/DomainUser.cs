using System.ComponentModel.DataAnnotations;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model
{
    public class DomainUser
    {
        [Required] public Guid Id { get; private set; }
        [Required] public string FirstName { get; set; } = null!;
        [Required] public string LastName { get; set; } = null!;
        public Gender Gender { get; set; } = Gender.Unspecified;
        public LocalDate? DateOfBirth { get; set; }

        [Required] public Guid AppUserId { get; private set; }
        public AppUser AppUser { get; set; } = null!;

        public string Discriminator { get; private set; } = null!;
    }
}
