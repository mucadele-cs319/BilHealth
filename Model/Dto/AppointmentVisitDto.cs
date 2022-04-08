using BilHealth.Utility.Enum;

namespace BilHealth.Model.Dto
{
    public record AppointmentVisitDto
    {
        public Guid Id { get; set; }
        public string Notes { get; set; } = String.Empty;
        public double? BPM { get; set; }
        public double? BloodPressure { get; set; }
        public double? BodyTemperature { get; set; }
    }
}
