using BilHealth.Model;
using BilHealth.Model.Dto;

namespace BilHealth.Services.Users
{
    public interface IProfileService
    {
        Task<UserProfileDto> GetFilteredUser(DomainUser requestingUser, Guid requestedUserId);

        Task<List<Case>> GetPastCases(DomainUser user);
        Task<List<Case>> GetOpenCases(DomainUser user);

        /// <param name="fullyEdit">Set to true if the sensitive details of the profile should be updated also</param>
        Task UpdateProfile(DomainUser user, UserProfileDto newProfile, bool fullyEdit = false);
        /// <param name="fullyEdit">Set to true if the sensitive details of the profile should be updated also</param>
        Task UpdateProfile(Guid userId, UserProfileDto newProfile, bool fullyEdit = false);
        Task SetPatientBlacklistState(Guid patientUserId, bool newState);

        Task AddVaccination(VaccinationDto details);
        Task<bool> UpdateVaccination(VaccinationDto details);
        Task<bool> RemoveVaccination(Guid vaccinationId);
    }
}
