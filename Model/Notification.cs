using System.ComponentModel.DataAnnotations;
using BilHealth.Utility.Enum;

namespace BilHealth.Model
{
    public class Notification
    {
        [Required] public Guid Id { get; private set; }
        [Required] public DateTime DateTime { get; set; }
        [Required] public Guid UserId { get; set; }
        public bool Read { get; set; } = false;
        [Required] public NotificationType Type { get; set; }

        // Refer to `NotificationType.cs` for the following two
        public Guid? ReferenceId1 { get; set; }
        public Guid? ReferenceId2 { get; set; }
    }
}
