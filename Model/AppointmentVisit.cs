using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace BilHealth.Model
{
    public class AppointmentVisit
    {
        [Required] public Guid Id { get; private set; }
        [Required] public Instant DateTime { get; set; }
        public string Notes { get; set; } = String.Empty;

        public double? BPM { get; set; }
        public double? BloodPressure { get; set; }
        public double? BodyTemperature { get; set; }

        [Required] public Guid AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }
    }
}
