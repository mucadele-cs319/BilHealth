using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services.Users
{
    public interface IProfileService
    {
        public Task SetSpecialization();
        public Task SetCampus();

        public Task AddVaccination();
        public Task UpdateVaccination();
        public Task<List<Case>> GetPastCases();
        public Task<List<Case>> GetOpenCases();
        public Task SetBodyWeight();
        public Task SetBodyHeight();
        public Task SetBloodType();
    }
}
