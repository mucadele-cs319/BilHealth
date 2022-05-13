using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto.Incoming;
using BilHealth.Services.Users;
using BilHealth.Utility.Enum;
using Microsoft.EntityFrameworkCore;
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
            await DbCtx.Entry(appointment).Reference(a => a.RequestingUser).LoadAsync();
            return appointment;
        }

        public async Task<bool> CancelAppointment(Guid appointmentId)
        {
            var appointment = await DbCtx.Appointments.FindAsync(appointmentId);
            if (appointment is null) return false;
            await DbCtx.Entry(appointment).Reference(a => a.Case).LoadAsync();

            appointment.Cancelled = true;
            NotificationService.AddAppointmentCancellationNotification(appointment.Case!.PatientUserId, appointment);

            await DbCtx.SaveChangesAsync();
            return true;
        }

        public async Task CreateVisit(Guid appointmentId)
        {
            var visitExists = await DbCtx.AppointmentVisits.Where(v => v.AppointmentId == appointmentId).AnyAsync();
            if (visitExists) return;

            var appointment = await DbCtx.Appointments.FindOrThrowAsync(appointmentId);
            appointment.Attended = true;

            var visit = new AppointmentVisit
            {
                AppointmentId = appointmentId,
                DateTime = Clock.GetCurrentInstant()
            };
            DbCtx.AppointmentVisits.Add(visit);
            await DbCtx.SaveChangesAsync();
        }

        public async Task SetAppointmentApproval(Guid appointmentId, ApprovalStatus approval)
        {
            var appointment = await DbCtx.Appointments.FindOrThrowAsync(appointmentId);

            appointment.ApprovalStatus = approval;
            await DbCtx.SaveChangesAsync();
        }

        public async Task<AppointmentVisit> UpdateVisit(Guid appointmentId, AppointmentVisitUpdateDto details)
        {
            var appointment = await DbCtx.Appointments.FindOrThrowAsync(appointmentId);
            await DbCtx.Entry(appointment).Reference(a => a.Visit).LoadAsync();

            if (appointment.Visit is null)
                throw new InvalidOperationException($"Cannot update nonexisting visit on appointment {appointmentId}");

            appointment.Visit.Notes = details.Notes ?? String.Empty;
            appointment.Visit.BodyTemperature = details.BodyTemperature;
            appointment.Visit.BloodPressure = details.BloodPressure;
            appointment.Visit.BPM = details.BPM;

            await DbCtx.SaveChangesAsync();
            return appointment.Visit;
        }
    }
}
