using BilHealth.Model.Dto;
using BilHealth.Utility;
using Microsoft.AspNetCore.Identity;

namespace BilHealth.Model
{
    public class AppUser : IdentityUser<Guid>
    {
        public AppUser() { }

        public AppUser(Registration registration)
        {
            UserName = registration.UserName; // Bilkent ID
            Email = registration.Email;

            DomainUser = DomainUserFactory.Create(registration);
        }

        public DomainUser DomainUser { get; set; } = null!;
    }
}
