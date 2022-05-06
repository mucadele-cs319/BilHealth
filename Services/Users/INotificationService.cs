using BilHealth.Model;

namespace BilHealth.Services.Users
{
    public interface INotificationService
    {
        Task AddNewAppointmentNotification(Appointment appointment);
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
        void AddCaseDoctorResignedNotification(Guid userId, Case _case);
        void AddNewPrescriptionNotification(Guid userId, Prescription prescription);
        void AddNewTestResultNotification(Guid userId, TestResult testResult);

        Task MarkNotificationRead(Guid notificationId);
        Task MarkAllNotificationsRead(DomainUser user);
        Task<List<Notification>> GetNotifications(DomainUser user, bool unread = false);
    }
}
