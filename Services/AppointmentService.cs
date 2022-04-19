using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Services.Users;
using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Services
{
    public class AppointmentService : DbServiceBase, IAppointmentService
    {
        private readonly INotificationService NotificationService;
        private readonly IClock Clock;

        public AppointmentService(AppDbContext dbCtx, INotificationService notificationService, IClock clock) : base(dbCtx)
        {
            NotificationService = notificationService;
            Clock = clock;
        }

        public async Task<Appointment> CreateAppointmentRequest(AppointmentDto details)
        {
            var _case = await DbCtx.Cases.FindAsync(details.CaseId);
            if (_case is null) throw new ArgumentException("Case is null");
            if (_case.DoctorUserId is null) throw new Exception("Case does not have a doctor yet");

            var appointment = new Appointment
            {
                CaseId = details.CaseId,
                RequestedById = details.RequestedById,
                ApprovalStatus = ApprovalStatus.Waiting,
                Attended = false,
                CreatedAt = Clock.GetCurrentInstant(),
                DateTime = details.DateTime,
                Description = details.Description
            };
            DbCtx.Appointments.Add(appointment);
            await NotificationService.AddNewAppointmentNotification(appointment);
            await DbCtx.SaveChangesAsync();
            return appointment;
        }

        public async Task<AppointmentVisit> CreateVisit(AppointmentVisitDto details)
        {
            var visit = new AppointmentVisit
            {
                AppointmentId = details.AppointmentId,
                BloodPressure = details.BloodPressure,
                BodyTemperature = details.BodyTemperature,
                BPM = details.BPM,
                Notes = details.Notes ?? String.Empty,
                DateTime = Clock.GetCurrentInstant()
            };
            DbCtx.AppointmentVisits.Add(visit);
            await DbCtx.SaveChangesAsync();
            return visit;
        }

        public async Task SetAppointmentApproval(Guid appointmentId, ApprovalStatus approval)
        {
            var appointment = await DbCtx.Appointments.FindAsync(appointmentId);
            if (appointment is null) throw new ArgumentException("No appointment with ID " + appointmentId);

            appointment.ApprovalStatus = approval;
            await DbCtx.SaveChangesAsync();
        }

        public async Task<AppointmentVisit> UpdatePatientVisitDetails(AppointmentVisitDto details)
        {
            var visit = await DbCtx.AppointmentVisits.FindAsync(details.Id);
            if (visit is null) throw new ArgumentException("No visit with ID " + details.Id);

            visit.Notes = details.Notes ?? visit.Notes;
            visit.BloodPressure = details.BloodPressure ?? visit.BloodPressure;
            visit.BodyTemperature = details.BodyTemperature ?? visit.BodyTemperature;
            visit.BPM = details.BPM ?? visit.BPM;
            await DbCtx.SaveChangesAsync();
            return visit;
        }
    }
}
