using NodaTime;

namespace BilHealth.Model.Dto
{
    public record VaccinationDto
    {
        public Guid Id { get; set; }
        public Guid PatientUserId { get; set; }

        public Instant? DateTime { get; set; }
        public string Type { get; set; } = String.Empty;
    }
}
