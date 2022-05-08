using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace BilHealth.Model.Dto.Incoming
{
    public class TimedAccessGrantCreateDto
    {
        [Required] public Period Period { get; set; } = null!;

        [Required] public Guid UserId { get; set; }
    }
}
