using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services.Users
{
    public interface IProfileService
    {
        Task<List<Case>> GetPastCases(User user);
        Task<List<Case>> GetOpenCases(User user);

        Task UpdateProfile(Patient patientUser, PatientProfileEdit newProfile);
        Task UpdateProfile(Doctor doctorUser, DoctorProfileEdit newProfile);
    }
}
