using System.Security.Claims;
using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility;
using Microsoft.AspNetCore.Identity;

namespace BilHealth.Services.Users
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> UserManager;
        private readonly SignInManager<User> SignInManager;
        private readonly RoleManager<Role> RoleManager;
        private readonly HttpContext? HttpContext;

        public AuthenticationService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            IHttpContextAccessor httpContextAccessor)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            HttpContext = httpContextAccessor.HttpContext;
        }

        public async Task<IdentityResult> Register(Registration registration)
        {
            var user = new User(registration);

            return await UserManager.CreateAsync(user, registration.Password);
        }

        public async Task RegisterMany(IList<Registration> registrations)
        {
            foreach (var registration in registrations)
                await Register(registration);
        }

        public async Task<SignInResult> LogIn(Login login)
        {
            var user = await UserManager.FindByNameAsync(login.UserName);
            if (user == null)
                return SignInResult.Failed;

            return await SignInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, lockoutOnFailure: false);
        }

        public async Task LogOut()
        {
            await SignInManager.SignOutAsync();
        }

        public async Task CreateRoles()
        {
            foreach (var roleName in UserRoles.Names)
                if (!await RoleManager.RoleExistsAsync(roleName))
                    await RoleManager.CreateAsync(new Role { Name = roleName });
        }

        public async Task<IdentityResult> AssignRole(User user, string roleName)
        {
            if (!UserRoles.Names.Contains(roleName))
                throw new ArgumentException("Assigning invalid role name", "roleName");

            return await UserManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> AssignRole(string userName, string roleName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null)
                return IdentityResult.Failed();

            return await AssignRole(user, roleName);
        }

        public async Task AssignRoles(User user, IEnumerable<string> roleNames)
        {
            foreach (var roleName in roleNames)
                await AssignRole(user, roleName);
        }

        public async Task AssignRoles(string userName, IEnumerable<string> roleNames)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user != null)
                await AssignRoles(user, roleNames);
        }

        public async Task<IdentityResult> DeleteUser(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user != null)
                return await UserManager.DeleteAsync(user);
            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> ChangePassword(User user, string currentPassword, string newPassword)
        {
            return await UserManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        // Unsafe: Only to be used for administration tasks
        public async Task<IdentityResult> ChangePasswordUnsafe(User user, string newPassword)
        {
            var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            return await UserManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<User> getUser(ClaimsPrincipal principal)
        {
            return await UserManager.GetUserAsync(principal);
        }
    }
}
