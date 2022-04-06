using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services
{
    public interface IAppointmentService
    {
        Task CreateAppointmentRequest(Appointment appointment);
        Task SetAppointmentApproval(Appointment appointment, ApprovalStatus approval);
        Task<AppointmentVisit> CreateVisit(AppointmentVisitDto details);

        Task<bool> SetNotesOfVisit(AppointmentVisitDto details);
        Task<bool> UpdatePatientVisitDetails(AppointmentVisitDto details);

        Task<bool> SetPatientBlacklistState(Guid patientUserId, bool newState);
        Task<bool> IsPatientBlacklisted(Patient patientUser);
    }
}
