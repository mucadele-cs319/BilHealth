using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Services.Users;
using BilHealth.Utility;
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

        public async Task<Appointment> CreateAppointment(AppointmentDto details)
        {
            var _case = await DbCtx.Cases.FindOrThrowAsync(details.CaseId);

            var requestingUser = await DbCtx.DomainUsers.FindOrThrowAsync(details.RequestedById);
            if (requestingUser is Patient patient && patient.Blacklisted)
                throw new InvalidOperationException($"Patient ({details.RequestedById}) is blacklisted from online appointments");

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

        public async Task<Appointment> UpdateAppointment(AppointmentDto details)
        {
            var appointment = await DbCtx.Appointments.FindOrThrowAsync(details.Id);
            await DbCtx.Entry(appointment).Reference(a => a.Case).LoadAsync();

            if (appointment.DateTime != details.DateTime)
                NotificationService.AddAppointmentTimeChangeNotification(appointment.Case!.PatientUserId, appointment);

            appointment.DateTime = details.DateTime;
            appointment.Description = details.Description;

            await DbCtx.SaveChangesAsync();
            return appointment;
        }

        public async Task<bool> CancelAppointment(Guid appointmentId)
        {
            var appointment = await DbCtx.Appointments.FindAsync(appointmentId);
            if (appointment is null) return false;
            await DbCtx.Entry(appointment).Reference(a => a.Case).LoadAsync();

            appointment.Cancelled = true;
            NotificationService.AddAppointmentCancellationNotification(appointment.Case!.PatientUserId, appointment);

            return true;
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
            var appointment = await DbCtx.Appointments.FindOrThrowAsync(appointmentId);

            appointment.ApprovalStatus = approval;
            await DbCtx.SaveChangesAsync();
        }

        public async Task<AppointmentVisit> UpdatePatientVisitDetails(AppointmentVisitDto details)
        {
            var visit = await DbCtx.AppointmentVisits.FindOrThrowAsync(details.Id);

            visit.Notes = details.Notes ?? visit.Notes;
            visit.BloodPressure = details.BloodPressure ?? visit.BloodPressure;
            visit.BodyTemperature = details.BodyTemperature ?? visit.BodyTemperature;
            visit.BPM = details.BPM ?? visit.BPM;
            await DbCtx.SaveChangesAsync();
            return visit;
        }
    }
}
