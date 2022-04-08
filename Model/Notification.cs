using System.ComponentModel.DataAnnotations;
using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model
{
    public class Notification
    {
        [Required] public Guid Id { get; private set; }
        [Required] public Instant DateTime { get; set; }
        [Required] public Guid UserId { get; set; }
        public bool Read { get; set; } = false;
        [Required] public NotificationType Type { get; set; }

        // Refer to the file `NotificationType.cs` for the meaning of the following two fields
        public Guid? ReferenceId1 { get; set; }
        public Guid? ReferenceId2 { get; set; }
    }
}
