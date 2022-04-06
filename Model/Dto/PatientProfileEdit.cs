using BilHealth.Utility.Enum;

namespace BilHealth.Model.Dto
{
    public record PatientProfileEdit
    {
        public double? BodyWeight { get; set; }
        public double? BodyHeight { get; set; }
        public BloodType? BloodType { get; set; }
    }
}
