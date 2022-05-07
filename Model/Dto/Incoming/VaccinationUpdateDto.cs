using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace BilHealth.Model.Dto.Incoming
{
    public class VaccinationUpdateDto
    {
        [Required] public Instant DateTime { get; set; }
        [Required] public string Type { get; set; } = null!;
    }
}
