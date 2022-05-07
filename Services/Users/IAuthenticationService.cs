using System.Security.Claims;
using BilHealth.Model;
using BilHealth.Model.Dto.Incoming;
using Microsoft.AspNetCore.Identity;

namespace BilHealth.Services.Users
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> Register(RegistrationDto registration);
        Task RegisterMany(IList<RegistrationDto> registrations);
        Task<SignInResult> LogIn(LoginDto login);
        Task LogOut();

        Task CreateRoles();
        Task<IdentityResult> AssignRole(AppUser user, string userType);

        Task<IdentityResult> DeleteUser(string userName);
        Task<IdentityResult> ChangePassword(AppUser user, string currentPassword, string newPassword);

        Task<DomainUser> GetUser(ClaimsPrincipal principal);
        Task<DomainUser> GetUser(Guid userId);
        string GetUserType(Guid userId);
        Task<List<DomainUser>> GetAllUsers();
    }
}
