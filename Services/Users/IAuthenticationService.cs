using System.Security.Claims;
using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;
using Microsoft.AspNetCore.Identity;

namespace BilHealth.Services.Users
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> Register(Registration registration);
        Task RegisterMany(IList<Registration> registrations);
        Task<SignInResult> LogIn(Login login);
        Task LogOut();

        Task CreateRoles();
        Task<IdentityResult> AssignRole(AppUser user, UserRoleType roleType);
        Task<IdentityResult> AssignRole(string userName, UserRoleType roleType);

        Task<IdentityResult> DeleteUser(string userName);
        Task<IdentityResult> ChangePassword(AppUser user, string currentPassword, string newPassword);
        Task<AppUser> GetAppUser(ClaimsPrincipal principal);
        Task<AppUser> GetAppUser(Guid userId);
        Task<List<AppUser>> GetAllAppUsers();
        Task<DomainUser> GetDomainUser(Guid userId);
        Task<UserRoleType> GetUserRole(AppUser user);
        UserRoleType GetUserRole(DomainUser user);
    }
}
