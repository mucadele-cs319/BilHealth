using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services
{
    public interface IAppointmentService
    {
        Task<Appointment> CreateAppointment(AppointmentDto details);
        Task SetAppointmentApproval(Guid appointmentId, ApprovalStatus approval);
        Task<AppointmentVisit> CreateVisit(AppointmentVisitDto details);

        Task<AppointmentVisit> UpdatePatientVisitDetails(AppointmentVisitDto details);
    }
}
