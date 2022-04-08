using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace BilHealth.Model
{
    public class Vaccination
    {
        [Required] public Guid Id { get; private set; }
        [Required] public Guid PatientUserId { get; set; }
        public Patient PatientUser { get; set; } = null!;

        public Instant? DateTime { get; set; }
        [Required] public string Type { get; set; } = null!;
    }
}
