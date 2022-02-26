using System.ComponentModel.DataAnnotations;

namespace BilHealth.Model
{
    public class Announcement
    {
        [Required] public Guid Id { get; private set; }
        [Required] public DateTime DateTime { get; set; }
        [Required] public string Title { get; set; } = null!;
        [Required] public string Message { get; set; } = null!;
    }
}
