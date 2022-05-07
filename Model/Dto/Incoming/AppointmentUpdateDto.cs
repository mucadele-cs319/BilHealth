using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace BilHealth.Model.Dto.Incoming
{
    public class AppointmentUpdateDto
    {
        [Required] public Instant DateTime { get; set; }
        public string Description { get; set; } = String.Empty;
    }
}
