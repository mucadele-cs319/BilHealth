using BilHealth.Model;
using Microsoft.AspNetCore.Identity;

namespace BilHealth.Services.Users
{
    public interface IAuthenticationService
    {
        public Task<IdentityResult> Register(Registration registration);
        public Task RegisterMany(IList<Registration> registrations);
        public Task<SignInResult> LogIn(Login login);
        public Task LogOut();

        public Task CreateRoles();
        public Task<IdentityResult> AssignRole(User user, string roleName);
        public Task AssignRoles(User user, IEnumerable<string> roleNames);
    }
}
