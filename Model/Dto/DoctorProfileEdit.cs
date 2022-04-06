using BilHealth.Utility.Enum;

namespace BilHealth.Model.Dto
{
    public record DoctorProfileEdit
    {
        public string Specialization { get; set; } = String.Empty;
        public Campus Campus { get; set; }
    }
}
