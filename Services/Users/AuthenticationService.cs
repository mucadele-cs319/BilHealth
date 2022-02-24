using BilHealth.Model;
using BilHealth.Model.Transaction;
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

        public async Task AssignRoles(User user, IEnumerable<string> roleNames)
        {
            foreach (var roleName in roleNames)
                await AssignRole(user, roleName);
        }
    }
}
