using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Utility.Enum;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace BilHealth.Services.Users
{
    public class NotificationService : DbServiceBase, INotificationService
    {
        private readonly IClock Clock;

        public NotificationService(AppDbContext dbCtx, IClock clock) : base(dbCtx)
        {
            Clock = clock;
        }

        private void AddNewAppointmentNotification(Guid userId, Appointment appointment)
        {
            var notification = new Notification
            {
                DateTime = Clock.GetCurrentInstant(),
                UserId = userId,
                Type = NotificationType.CaseNewAppointment,
                ReferenceId1 = appointment.CaseId,
                ReferenceId2 = appointment.Id
            };
            DbCtx.Notifications.Add(notification);
        }

        public async Task AddNewAppointmentNotification(Appointment appointment)
        {
            var user = await DbCtx.DomainUsers.FindOrThrowAsync(appointment.RequestedById);
            await DbCtx.Entry(appointment).Reference(a => a.Case).LoadAsync();
            if (appointment.Case!.DoctorUserId is null)
                throw new InvalidOperationException($"Case ({appointment.Case.Id}) is missing a doctor");

            switch (user)
            {
                case Patient:
                    AddNewAppointmentNotification(appointment.Case.DoctorUserId.Value, appointment);
                    break;
                case Doctor:
                    AddNewAppointmentNotification(appointment.Case.PatientUserId, appointment);
                    break;
                default:
                    AddNewAppointmentNotification(appointment.Case.DoctorUserId.Value, appointment);
                    AddNewAppointmentNotification(appointment.Case.PatientUserId, appointment);
                    break;
            }
        }

        public void AddAppointmentTimeChangeNotification(Guid userId, Appointment appointment)
        {
            var notification = new Notification
            {
                DateTime = Clock.GetCurrentInstant(),
                UserId = userId,
                Type = NotificationType.CaseAppointmentTimeChanged,
                ReferenceId1 = appointment.CaseId,
                ReferenceId2 = appointment.Id
            };
            DbCtx.Notifications.Add(notification);
        }

        public void AddAppointmentCancellationNotification(Guid userId, Appointment appointment)
        {
            var notification = new Notification
            {
                DateTime = Clock.GetCurrentInstant(),
                UserId = userId,
                Type = NotificationType.CaseAppointmentCanceled,
                ReferenceId1 = appointment.CaseId,
                ReferenceId2 = appointment.Id
            };
            DbCtx.Notifications.Add(notification);
        }

        private void AddNewCaseMessageNotification(Guid userId, CaseMessage message)
        {
            var notification = new Notification
            {
                DateTime = Clock.GetCurrentInstant(),
                UserId = userId,
                Type = NotificationType.CaseNewMessage,
                ReferenceId1 = message.CaseId,
                ReferenceId2 = message.Id
            };
            DbCtx.Notifications.Add(notification);
        }

        public async Task AddNewCaseMessageNotification(CaseMessage message)
        {
            var user = await DbCtx.DomainUsers.SingleOrDefaultAsync(user => user.AppUser.Id == message.UserId);
            if (user is null) return;

            await DbCtx.Entry(message).Reference(m => m.Case).LoadAsync();
            if (message.Case is null) throw new Exception("Case is null");

            if (user is Patient && message.Case.DoctorUserId is not null)
            {
                AddNewCaseMessageNotification(message.Case.DoctorUserId.Value, message);
            }
            else if (user is Doctor)
            {
                AddNewCaseMessageNotification(message.Case.PatientUserId, message);
            }
            else
            {
                if (message.Case.DoctorUserId is not null)
                    AddNewCaseMessageNotification(message.Case.DoctorUserId.Value, message);
                AddNewCaseMessageNotification(message.Case.PatientUserId, message);
            }
        }

        public void AddCaseClosedNotification(Guid userId, Case _case)
        {
            var notification = new Notification
            {
                DateTime = Clock.GetCurrentInstant(),
                UserId = userId,
                Type = NotificationType.CaseClosed,
                ReferenceId1 = _case.Id
            };
            DbCtx.Notifications.Add(notification);
        }

        public void AddCaseTriagedNotification(Guid userId, Case _case)
        {
            var notification = new Notification
            {
                DateTime = Clock.GetCurrentInstant(),
                UserId = userId,
                Type = NotificationType.CaseTriaged,
                ReferenceId1 = _case.Id
            };
            DbCtx.Notifications.Add(notification);
        }

        public void AddCaseDoctorResignedNotification(Guid userId, Case _case)
        {
            var notification = new Notification
            {
                DateTime = Clock.GetCurrentInstant(),
                UserId = userId,
                Type = NotificationType.CaseDoctorResigned,
                ReferenceId1 = _case.Id
            };
            DbCtx.Notifications.Add(notification);
        }

        public void AddNewPrescriptionNotification(Guid userId, Prescription prescription)
        {
            var notification = new Notification
            {
                DateTime = Clock.GetCurrentInstant(),
                UserId = userId,
                Type = NotificationType.CaseNewPrescription,
                ReferenceId1 = prescription.CaseId,
                ReferenceId2 = prescription.Id
            };
            DbCtx.Notifications.Add(notification);
        }

        public void AddNewTestResultNotification(Guid userId, TestResult testResult)
        {
            var notification = new Notification
            {
                DateTime = Clock.GetCurrentInstant(),
                UserId = userId,
                Type = NotificationType.TestResultNew,
                ReferenceId1 = testResult.Id
            };
            DbCtx.Notifications.Add(notification);
        }

        public async Task MarkNotificationRead(Guid notificationId)
        {
            var notification = await DbCtx.Notifications.SingleOrDefaultAsync(notification => notification.Id == notificationId);
            if (notification is null) return;

            notification.Read = true;
            await DbCtx.SaveChangesAsync();
        }

        public async Task MarkAllNotificationsRead(DomainUser user)
        {
            await DbCtx.Notifications.Where(n => n.UserId == user.Id && n.Read == false).ForEachAsync(n => n.Read = true);
            await DbCtx.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetNotifications(DomainUser user, bool unread)
        {
            return await DbCtx.Notifications.Where(n => n.UserId == user.Id && (!unread || n.Read == false))
                .OrderByDescending(n => n.DateTime).ToListAsync();
        }
    }
}
