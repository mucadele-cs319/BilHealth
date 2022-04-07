using BilHealth.Data;
using BilHealth.Model;
using BilHealth.Utility.Enum;
using Microsoft.EntityFrameworkCore;

namespace BilHealth.Services.Users
{
    public class NotificationService : DbServiceBase, INotificationService
    {
        public NotificationService(AppDbContext dbCtx) : base(dbCtx)
        {
        }

        public void AddNewAppointmentNotification(Guid userId, Appointment appointment)
        {
            var notification = new Notification
            {
                DateTime = DateTime.Now,
                UserId = userId,
                Type = NotificationType.CaseNewAppointment,
                ReferenceId1 = appointment.CaseId,
                ReferenceId2 = appointment.Id
            };
            DbCtx.Notifications.Add(notification);
        }

        public void AddAppointmentTimeChangeNotification(Guid userId, Appointment appointment)
        {
            var notification = new Notification
            {
                DateTime = DateTime.Now,
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
                DateTime = DateTime.Now,
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
                DateTime = DateTime.Now,
                UserId = userId,
                Type = NotificationType.CaseNewMessage,
                ReferenceId1 = message.CaseId,
                ReferenceId2 = message.Id
            };
            DbCtx.Notifications.Add(notification);
        }

        public async Task AddNewCaseMessageNotification(CaseMessage message)
        {
            var user = await DbCtx.DomainUsers.SingleOrDefaultAsync(user => user.Id == message.UserId);
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
                DateTime = DateTime.Now,
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
                DateTime = DateTime.Now,
                UserId = userId,
                Type = NotificationType.CaseTriaged,
                ReferenceId1 = _case.Id
            };
            DbCtx.Notifications.Add(notification);
        }

        public void AddCaseDoctorChangedNotification(Guid userId, Case _case)
        {
            var notification = new Notification
            {
                DateTime = DateTime.Now,
                UserId = userId,
                Type = NotificationType.CaseDoctorChanged,
                ReferenceId1 = _case.Id
            };
            DbCtx.Notifications.Add(notification);
        }

        public void AddNewPrescriptionNotification(Guid userId, Prescription prescription)
        {
            var notification = new Notification
            {
                DateTime = DateTime.Now,
                UserId = userId,
                Type = NotificationType.CaseNewPrescription,
                ReferenceId1 = prescription.CaseId,
                ReferenceId2 = prescription.Id
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
            var unreads = await DbCtx.Notifications.Where(n => n.UserId == user.Id && n.Read == false).ToListAsync();
            foreach (var unread in unreads)
                unread.Read = true;
            await DbCtx.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetUnreadNotifications(DomainUser user)
        {
            return await DbCtx.Notifications.Where(n => n.UserId == user.Id && n.Read == false).OrderByDescending(n => n.DateTime).ToListAsync();
        }

        public async Task<List<Notification>> GetAllNotifications(DomainUser user)
        {
            return await DbCtx.Notifications.Where(n => n.UserId == user.Id).OrderByDescending(n => n.DateTime).ToListAsync();
        }
    }
}
