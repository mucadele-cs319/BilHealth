using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services
{
    public interface IAppointmentService
    {
        public Task CreateAppointmentRequest();
        public Task SetAppointmentApproval();
        public Task<AppointmentVisit> CreateVisit();

        public Task SetNotesOfVisit();
        public Task SetBPM();
        public Task SetBloodPressure();
        public Task SetBloodTemperature();

        public Task<bool> IsPatientBlacklisted();
    }
}
