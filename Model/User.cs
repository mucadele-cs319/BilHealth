using BilHealth.Model.Dto;
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

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
