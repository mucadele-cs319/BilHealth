namespace BilHealth.Model.Dto
{
    public record AppointmentVisitDto
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public string? Notes { get; set; }
        public double? BPM { get; set; }
        public double? BloodPressure { get; set; }
        public double? BodyTemperature { get; set; }
    }
}
