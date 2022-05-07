namespace BilHealth.Model.Dto.Incoming
{
    public class AppointmentVisitUpdateDto
    {
        public string? Notes { get; set; }
        public double? BPM { get; set; }
        public double? BloodPressure { get; set; }
        public double? BodyTemperature { get; set; }
    }
}
