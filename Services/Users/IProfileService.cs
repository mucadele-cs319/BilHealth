using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services.Users
{
    public interface IProfileService
    {
        Task<List<Case>> GetPastCases(DomainUser user);
        Task<List<Case>> GetOpenCases(DomainUser user);

        Task UpdateProfile(Patient patientUser, PatientProfileEdit newProfile);
        Task UpdateProfile(Doctor doctorUser, DoctorProfileEdit newProfile);
    }
}
