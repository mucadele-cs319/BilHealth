using System.ComponentModel.DataAnnotations;

namespace BilHealth.Model.Dto.Incoming
{
    public class AnnouncementUpdateDto
    {
        [Required] public string Title { get; set; } = null!;
        [Required] public string Message { get; set; } = null!;
    }
}
