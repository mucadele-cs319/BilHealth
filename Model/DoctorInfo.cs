using System.ComponentModel.DataAnnotations;
using BilHealth.Utility.Enum;

namespace BilHealth.Model
{
    public class DoctorInfo
    {
        [Required] public Guid Id { get; private set; }
        public string Specialization { get; set; } = String.Empty;
        public Campus Campus { get; set; }
    }
}
