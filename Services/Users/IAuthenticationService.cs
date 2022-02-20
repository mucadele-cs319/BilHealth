using BilHealth.Model;
using Microsoft.AspNetCore.Identity;

namespace BilHealth.Services.Users
{
    public interface IAuthenticationService
    {
        public Task<IdentityResult> Register(string username, string password, string email);
        public Task RegisterMany(IList<User> users, IList<string> passwords);
        public Task<SignInResult> LogIn(User user, string password, bool persist);
        public Task LogOut();

        public Task CreateRoles();
        public Task<IdentityResult> AssignRole(User user, string roleName);
        public Task AssignRoles(User user, IEnumerable<string> roleNames);
    }
}
