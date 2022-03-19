using BilHealth.Utility.Enum;

namespace BilHealth.Model
{
    public class Doctor : User
    {
        public string Specialization { get; set; } = String.Empty;
        public Campus Campus { get; set; }
    }
}
