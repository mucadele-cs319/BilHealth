using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Model.Dto.Incoming;
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

        public async Task<Appointment> CreateAppointment(Guid caseId, Guid requestingUserId, AppointmentUpdateDto details)
        {
            var _case = await DbCtx.Cases.FindOrThrowAsync(caseId);

            var requestingUser = await DbCtx.DomainUsers.FindOrThrowAsync(requestingUserId);
            if (requestingUser is Patient patient && patient.Blacklisted)
                throw new InvalidOperationException($"Patient ({requestingUserId}) is blacklisted from online appointments");

            var appointment = new Appointment
            {
                CaseId = caseId,
                RequestingUserId = requestingUserId,
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

        public async Task<Appointment> UpdateAppointment(Guid appointmentId, AppointmentUpdateDto details)
        {
            var appointment = await DbCtx.Appointments.FindOrThrowAsync(appointmentId);
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

        public async Task<AppointmentVisit> CreateVisit(Guid appointmentId, AppointmentVisitUpdateDto details)
        {
            var visit = new AppointmentVisit
            {
                AppointmentId = appointmentId,
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

        public async Task<AppointmentVisit> UpdatePatientVisitDetails(Guid appointmentId, AppointmentVisitUpdateDto details)
        {
            var appointment = await DbCtx.Appointments.FindOrThrowAsync(appointmentId);
            await DbCtx.Entry(appointment).Reference(a => a.Visit).LoadAsync();

            var visit = appointment.Visit;
            if (visit is null) throw new InvalidOperationException($"Cannot update nonexisting visit on appointment {appointmentId}");

            visit.Notes = details.Notes ?? visit.Notes;
            visit.BloodPressure = details.BloodPressure ?? visit.BloodPressure;
            visit.BodyTemperature = details.BodyTemperature ?? visit.BodyTemperature;
            visit.BPM = details.BPM ?? visit.BPM;
            await DbCtx.SaveChangesAsync();
            return visit;
        }
    }
}
