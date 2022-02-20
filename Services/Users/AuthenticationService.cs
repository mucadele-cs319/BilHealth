using BilHealth.Model;
using BilHealth.Utility;
using Microsoft.AspNetCore.Identity;

namespace BilHealth.Services.Users
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> UserManager;
        private readonly SignInManager<User> SignInManager;
        private readonly RoleManager<Role> RoleManager;

        public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public async Task<IdentityResult> Register(string username, string password, string email)
        {
            var user = new User
            {
                UserName = username,
                Email = email
            };

            var result = await UserManager.CreateAsync(user, password);

            if (result.Succeeded)
                await SignInManager.SignInAsync(user, false);

            return result;
        }

        public async Task RegisterMany(IList<User> users, IList<string> passwords)
        {
            for (int i = 0; i != users.Count; i++)
                await Register(users[i].UserName, passwords[i], users[i].Email);
        }

        public async Task<SignInResult> LogIn(User user, string password, bool persist)
        {
            return await SignInManager.PasswordSignInAsync(user, password, persist, false);
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
