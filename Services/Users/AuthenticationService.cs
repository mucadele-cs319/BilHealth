using System.Security.Claims;
using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto.Incoming;
using BilHealth.Utility.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IdentityResult> Register(RegistrationDto registration)
        {
            var roleType = UserType.Names.First(roleType => roleType == registration.UserType);
            if (roleType is null) return IdentityResult.Failed(new IdentityError { Description = "Invalid role" });

            AppUser user = new(registration);

            var creationResult = await UserManager.CreateAsync(user, registration.Password);
            return creationResult.Succeeded ? await AssignRole(user, roleType) : creationResult;
        }

        public async Task RegisterMany(IList<RegistrationDto> registrations)
        {
            foreach (var registration in registrations)
                await Register(registration);
        }

        public async Task<SignInResult> LogIn(LoginDto login)
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
            foreach (var roleName in UserType.Names)
                if (!await RoleManager.RoleExistsAsync(roleName))
                    await RoleManager.CreateAsync(new Role { Name = roleName });
        }

        public Task<IdentityResult> AssignRole(AppUser user, string userType)
        {
            return UserManager.AddToRoleAsync(user, userType);
        }

        public async Task<IdentityResult> DeleteUser(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            return user is null ? IdentityResult.Failed() : await UserManager.DeleteAsync(user);
        }

        public Task<IdentityResult> ChangePassword(AppUser user, string currentPassword, string newPassword)
        {
            return UserManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        /// <summary>
        /// Unsafe: To be used strictly for administration tasks
        /// </summary>
        public async Task<IdentityResult> ChangePasswordUnsafe(AppUser user, string newPassword)
        {
            var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            return await UserManager.ResetPasswordAsync(user, token, newPassword);
        }

        private async Task<DomainUser> LoadUserNavigationProps(DomainUser user)
        {
            switch (user)
            {
                case Doctor doctor:
                    await DbCtx.Entry(doctor).Collection(d => d.Cases!).LoadAsync();
                    break;
                case Patient patient:
                    await DbCtx.Entry(patient).Collection(p => p.Vaccinations!).LoadAsync();
                    await DbCtx.Entry(patient).Collection(p => p.TestResults!).LoadAsync();
                    await DbCtx.Entry(patient).Collection(p => p.Cases!).LoadAsync();
                    break;
            }
            return user;
        }

        public async Task<DomainUser> GetUser(ClaimsPrincipal principal)
        {
            var user = await UserManager.GetUserAsync(principal);
            return await LoadUserNavigationProps(user.DomainUser);
        }

        public async Task<DomainUser> GetUser(Guid userId) =>
            await LoadUserNavigationProps(await DbCtx.DomainUsers.FindOrThrowAsync(userId));

        public Task<string> GetUserType(Guid userId) =>
            DbCtx.DomainUsers.Where(u => u.Id == userId).Select(u => u.Discriminator).SingleOrDefaultAsync()!;

        /// <summary>
        /// Only loads the <see cref="AppUser"/> navigation property.
        /// </summary>
        /// <remarks>
        /// This is not scalable. Ideally, pagination would be used.
        /// </remarks>
        /// <returns>List of all <see cref="DomainUser"/>s</returns>
        public Task<List<DomainUser>> GetAllUsers() => DbCtx.DomainUsers.ToListAsync();
    }
}
