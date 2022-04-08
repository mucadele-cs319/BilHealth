using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Utility
{
    public class DomainUserFactory
    {
        public static DomainUser Create(Registration registration)
        {
            var userType = UserRoleType.Names.First(roleType => roleType == registration.UserType);
            return Create(userType, u =>
            {
                u.FirstName = registration.FirstName;
                u.LastName = registration.LastName;
            });
        }

        public static DomainUser Create(UserRoleType userType, Action<DomainUser> initializer)
        {
            DomainUser domainUser;

            if (userType == UserRoleType.Admin)
            {
                domainUser = new Admin();
            }
            else if (userType == UserRoleType.Doctor)
            {
                domainUser = new Doctor();
            }
            else if (userType == UserRoleType.Nurse)
            {
                domainUser = new Nurse();
            }
            else if (userType == UserRoleType.Staff)
            {
                domainUser = new Staff();
            }
            else if (userType == UserRoleType.Patient)
            {
                domainUser = new Patient();
            }
            else
            {
                throw new ArgumentException("Invalid user type", nameof(userType));
            }

            initializer(domainUser);
            return domainUser;
        }
    }
}
