using BilHealth.Model;
using BilHealth.Model.Dto.Incoming;

namespace BilHealth.Services.Users
{
    public interface IProfileService
    {
        Task<List<Case>> GetPastCases(DomainUser user);
        Task<List<Case>> GetOpenCases(DomainUser user);

        /// <param name="fullyEdit">Set to true if the sensitive details of the profile should be updated also</param>
        Task UpdateProfile(DomainUser user, UserProfileUpdateDto details, bool fullyEdit = false);
        /// <param name="fullyEdit">Set to true if the sensitive details of the profile should be updated also</param>
        Task UpdateProfile(Guid userId, UserProfileUpdateDto details, bool fullyEdit = false);
        Task SetPatientBlacklistState(Guid patientUserId, bool newState);

        Task AddVaccination(Guid patientUserId, VaccinationUpdateDto details);
        Task<bool> UpdateVaccination(Guid vaccinationId, VaccinationUpdateDto details);
        Task<bool> RemoveVaccination(Guid vaccinationId);
    }
}
