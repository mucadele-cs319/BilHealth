using BilHealth.Model;

namespace BilHealth.Services.Users
{
    public interface INotificationService
    {
        void AddNewAppointmentNotification(Guid userId, Appointment appointment);
        void AddAppointmentTimeChangeNotification(Guid userId, Appointment appointment);
        void AddAppointmentCancellationNotification(Guid userId, Appointment appointment);
        /// <summary>
        /// Add a new notification for a new message.
        /// <br/>
        /// If the message is from a Patient and the case has an assigned Doctor, the notification is given to the Doctor.
        /// If the message is from a Doctor, the notification is given to the Patient.
        /// Otherwise, both Patient and Doctor (if assigned) are given a notification.
        /// </summary>
        /// <param name="message">The new message</param>
        /// <returns>Completed task after DB queries</returns>
        Task AddNewCaseMessageNotification(CaseMessage message);
        void AddCaseClosedNotification(Guid userId, Case _case);
        void AddCaseTriagedNotification(Guid userId, Case _case);
        void AddCaseDoctorChangedNotification(Guid userId, Case _case);
        void AddNewPrescriptionNotification(Guid userId, Prescription prescription);

        Task MarkNotificationRead(Guid notificationId);
        Task MarkAllNotificationsRead(User user);
        Task<List<Notification>> GetUnreadNotifications(User user);
        Task<List<Notification>> GetAllNotifications(User user);
    }
}
