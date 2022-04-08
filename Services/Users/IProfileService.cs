using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services.Users
{
    public interface IProfileService
    {
        Task<List<Case>> GetPastCases(DomainUser user);
        Task<List<Case>> GetOpenCases(DomainUser user);

        Task UpdateProfile(DomainUser patientUser, UserProfileDto newProfile);

        Task AddVaccination(VaccinationDto details);
        Task<bool> UpdateVaccination(VaccinationDto details);
        Task<bool> RemoveVaccination(Guid vaccinationId);
    }
}
