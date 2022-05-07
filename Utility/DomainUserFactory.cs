using BilHealth.Model;
using BilHealth.Model.Dto.Incoming;
using BilHealth.Utility.Enum;

namespace BilHealth.Utility
{
    public class DomainUserFactory
    {
        public static DomainUser Create(RegistrationDto registration) =>
            Create(registration.UserType, u =>
            {
                u.FirstName = registration.FirstName;
                u.LastName = registration.LastName;
            });

        public static DomainUser Create(string userType, Action<DomainUser> initializer)
        {
            DomainUser domainUser = userType switch
            {
                UserType.Admin => new Admin(),
                UserType.Staff => new Staff(),
                UserType.Doctor => new Doctor(),
                UserType.Nurse => new Nurse(),
                UserType.Patient => new Patient(),
                _ => throw new ArgumentOutOfRangeException("Invalid user type", nameof(userType)),
            };

            initializer(domainUser);
            return domainUser;
        }
    }
}
