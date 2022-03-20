using System.ComponentModel.DataAnnotations;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;
using Microsoft.AspNetCore.Identity;

namespace BilHealth.Model
{
    public class User : IdentityUser<Guid>
    {
        public User() { }

        public User(Registration registration)
        {
            UserName = registration.UserName; // Bilkent ID
            FirstName = registration.FirstName;
            LastName = registration.LastName;
            Email = registration.Email;
        }

        [Required] public string FirstName { get; set; } = null!;
        [Required] public string LastName { get; set; } = null!;
        public Gender Gender { get; set; } = Gender.Unspecified;
        public DateTime? DateOfBirth { get; set; }

        public List<Notification>? Notifications { get; set; }
    }
}
