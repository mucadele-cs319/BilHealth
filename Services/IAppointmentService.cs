using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services
{
    public interface IAppointmentService
    {
        Task CreateAppointmentRequest(Appointment appointment);
        Task SetAppointmentApproval(Guid appointmentId, ApprovalStatus approval);
        Task<AppointmentVisit> CreateVisit(AppointmentVisitDto details);

        Task<bool> UpdatePatientVisitDetails(AppointmentVisitDto details);

        Task<bool> SetPatientBlacklistState(Guid patientUserId, bool newState);
    }
}
