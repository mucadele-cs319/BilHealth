using System.ComponentModel.DataAnnotations;

namespace BilHealth.Model
{
    public class AppointmentVisit
    {
        [Required] public Guid Id { get; private set; }
        [Required] public DateTime DateTime { get; set; }
        [Required] public string Notes { get; set; } = null!;

        public string? BPM { get; set; }
        public string? BloodPressure { get; set; }
        public string? BodyTemperature { get; set; }

        [Required] public Guid AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }
    }
}
