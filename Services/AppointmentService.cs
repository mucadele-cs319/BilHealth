using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Services
{
    public class AppointmentService : DbServiceBase, IAppointmentService
    {
        private readonly IClock Clock;

        public AppointmentService(AppDbContext dbCtx, IClock clock) : base(dbCtx)
        {
            Clock = clock;
        }

        public async Task<Appointment> CreateAppointmentRequest(AppointmentDto details)
        {
            var appointment = new Appointment
            {
                CaseId = details.CaseId,
                ApprovalStatus = ApprovalStatus.Waiting,
                Attended = false,
                CreatedAt = Clock.GetCurrentInstant(),
                DateTime = details.DateTime,
                Description = details.Description
            };
            DbCtx.Appointments.Add(appointment);
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

        public async Task SetPatientBlacklistState(Guid patientUserId, bool newState)
        {
            var patientUser = await DbCtx.Users.FindAsync(patientUserId);
            if (patientUser is null) throw new ArgumentException("No patient user with ID " + patientUserId);

            if (patientUser.DomainUser is Patient patient)
                    patient.Blacklisted = newState;
            else throw new ArgumentException($"Given ID {patientUserId} belongs to non-patient user");

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
