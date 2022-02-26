using System.ComponentModel.DataAnnotations;

namespace BilHealth.Model
{
    public class Appointment
    {
        [Required] public Guid Id { get; private set; }
        [Required] public DateTime CreatedAt { get; set; }
        [Required] public DateTime DateTime { get; set; }
        [Required] public string Description { get; set; } = null!;

        [Required] public Guid CaseId { get; set; }
        public Case? Case { get; set; }
    }
}
