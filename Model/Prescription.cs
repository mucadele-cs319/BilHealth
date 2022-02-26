using System.ComponentModel.DataAnnotations;

namespace BilHealth.Model
{
    public class Prescription
    {
        [Required] public Guid Id { get; private set; }
        [Required] public DateTime DateTime { get; set; }
        [Required] public Guid CaseId { get; set; }
        public Case? Case { get; set; }
        [Required] public Guid DoctorUserId { get; set; }
        public User? DoctorUser { get; set; }

        [Required] public string Item { get; set; } = null!; // TODO: Need a proper way to store drugs
    }
}
