using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace BilHealth.Model
{
    public class Announcement
    {
        [Required] public Guid Id { get; private set; }
        [Required] public Instant DateTime { get; set; }
        [Required] public string Title { get; set; } = null!;
        [Required] public string Message { get; set; } = null!;
    }
}
