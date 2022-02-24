using System.Security.Claims;
using BilHealth.Model;
using BilHealth.Model.Transaction;
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
        public Task<IdentityResult> AssignRole(string userName, string roleName);
        public Task AssignRoles(User user, IEnumerable<string> roleNames);
        public Task AssignRoles(string userName, IEnumerable<string> roleNames);

        public Task<IdentityResult> DeleteUser(string userName);
        public Task<IdentityResult> ChangePassword(User user, string currentPassword, string newPassword);
        public Task<User> getUser(ClaimsPrincipal principal);
    }
}
