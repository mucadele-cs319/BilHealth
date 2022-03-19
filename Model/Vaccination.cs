using System.ComponentModel.DataAnnotations;

namespace BilHealth.Model
{
    public class Vaccination
    {
        [Required] public Guid Id { get; private set; }
        [Required] public Guid PatientUserId { get; set; }
        public User PatientUser { get; set; } = null!;

        public DateTime? DateTime { get; set; }
        [Required] public string Type { get; set; } = null!;
    }
}
