using BilHealth.Utility.Enum;

namespace BilHealth.Model
{
    public class Doctor : DomainUser
    {
        public string Specialization { get; set; } = String.Empty;
        public Campus Campus { get; set; }

        // [InverseProperty("DoctorUser")] public List<Case>? Cases { get; set; }
    }
}
