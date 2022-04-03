using BilHealth.Model;
using BilHealth.Model.Dto;
using BilHealth.Utility.Enum;

namespace BilHealth.Services.Users
{
    public interface INotificationService
    {
        public void AddNewAppointmentNotification(Guid userId, Appointment appointment);
        public void AddAppointmentTimeChangeNotification(Guid userId, Appointment appointment);
        public void AddAppointmentCancellationNotification(Guid userId, Appointment appointment);
        public Task AddNewCaseMessageNotification(CaseMessage message);
        public void AddCaseClosedNotification(Guid userId, Case _case);
        public void AddCaseTriagedNotification(Guid userId, Case _case);
        public void AddCaseDoctorChangedNotification(Guid userId, Case _case);
        public void AddNewPrescriptionNotification(Guid userId, Prescription prescription);

        public Task MarkNotificationRead(Guid notificationId);
        public Task MarkAllNotificationsRead(User user);
        public Task<List<Notification>> GetUnreadNotifications(User user);
        public Task<List<Notification>> GetAllNotifications(User user);
    }
}
