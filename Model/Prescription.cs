using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace BilHealth.Model
{
    public class Prescription
    {
        [Required] public Guid Id { get; private set; }
        [Required] public Instant DateTime { get; set; }
        [Required] public Guid CaseId { get; set; }
        public Case Case { get; set; } = null!;
        [Required] public Guid DoctorUserId { get; set; }
        public Doctor DoctorUser { get; set; } = null!;

        [Required] public string Item { get; set; } = null!; // TODO: Need a proper way to store drugs
    }
}
