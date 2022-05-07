using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model.Dto
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public Instant DateTime { get; set; }
        public Guid UserId { get; set; }
        public bool Read { get; set; } = false;
        public NotificationType Type { get; set; }

        // Refer to the file `NotificationType.cs` for the meaning of the following two fields
        public Guid? ReferenceId1 { get; set; } = null;
        public Guid? ReferenceId2 { get; set; } = null;
    }
}
