using System.Security.Claims;
using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;
using Microsoft.AspNetCore.Identity;

namespace BilHealth.Services.Users
{
    public class AuthenticationService : DbServiceBase, IAuthenticationService
    {
        private readonly UserManager<AppUser> UserManager;
        private readonly SignInManager<AppUser> SignInManager;
        private readonly RoleManager<Role> RoleManager;
        private readonly HttpContext? HttpContext;

        public AuthenticationService(
            AppDbContext dbCtx,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<Role> roleManager,
            IHttpContextAccessor httpContextAccessor) : base(dbCtx)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            HttpContext = httpContextAccessor.HttpContext;
        }

        public async Task<IdentityResult> Register(Registration registration)
        {
            var roleType = UserRoleType.Names.First(roleType => roleType == registration.UserType);
            if (roleType is null) return IdentityResult.Failed(new IdentityError { Description = "Invalid role" });

            AppUser user = new(registration);

            var creationResult = await UserManager.CreateAsync(user, registration.Password);
            return creationResult.Succeeded ? await AssignRole(user, roleType) : creationResult;
        }

        public async Task RegisterMany(IList<Registration> registrations)
        {
            foreach (var registration in registrations)
                await Register(registration);
        }

        public async Task<SignInResult> LogIn(Login login)
        {
            var user = await UserManager.FindByNameAsync(login.UserName);
            if (user is null) return SignInResult.Failed;

            return await SignInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, lockoutOnFailure: false);
        }

        public async Task LogOut()
        {
            await SignInManager.SignOutAsync();
        }

        public async Task CreateRoles()
        {
            foreach (var roleName in UserRoleType.Names)
                if (!await RoleManager.RoleExistsAsync(roleName))
                    await RoleManager.CreateAsync(new Role { Name = roleName });
        }

        public async Task<IdentityResult> AssignRole(AppUser user, UserRoleType roleType)
        {
            return await UserManager.AddToRoleAsync(user, roleType);
        }

        public async Task<IdentityResult> AssignRole(string userName, UserRoleType roleType)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user is null) return IdentityResult.Failed();

            return await AssignRole(user, roleType);
        }

        public async Task<IdentityResult> DeleteUser(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            return user is null ? IdentityResult.Failed() : await UserManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> ChangePassword(AppUser user, string currentPassword, string newPassword)
        {
            return await UserManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        /// <summary>
        /// Unsafe: Only to be used for administration tasks
        /// </summary>
        public async Task<IdentityResult> ChangePasswordUnsafe(AppUser user, string newPassword)
        {
            var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            return await UserManager.ResetPasswordAsync(user, token, newPassword);
        }

        private async Task LoadUser(AppUser user)
        {
            await DbCtx.Entry(user).Reference(u => u.DomainUser).LoadAsync();

            switch (user.DomainUser)
            {
                case Doctor doctor:
                    await DbCtx.Entry(doctor).Collection(d => d.Cases!).LoadAsync();
                    break;
                case Nurse nurse:
                    await DbCtx.Entry(nurse).Collection(n => n.TriageRequests!).LoadAsync();
                    break;
                case Patient patient:
                    await DbCtx.Entry(patient).Collection(p => p.Vaccinations!).LoadAsync();
                    await DbCtx.Entry(patient).Collection(p => p.TestResults!).LoadAsync();
                    await DbCtx.Entry(patient).Collection(p => p.Cases!).LoadAsync();
                    break;
            }
        }

        public async Task<AppUser> GetUser(ClaimsPrincipal principal)
        {
            var user = await UserManager.GetUserAsync(principal);
            await LoadUser(user);
            return user;
        }

        public async Task<AppUser> GetUser(Guid userId)
        {
            var user = await DbCtx.Users.FindAsync(userId);
            if (user is null) throw new ArgumentException("No user found with id" + userId);
            await LoadUser(user);
            return user;
        }

        public async Task<UserRoleType> GetUserRole(AppUser user)
        {
            var roleName = (await UserManager.GetRolesAsync(user)).First();
            return UserRoleType.Names.First(roleType => roleType == roleName);
        }
    }
}
