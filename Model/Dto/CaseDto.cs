using BilHealth.Utility.Enum;

namespace BilHealth.Model.Dto
{
    public record CaseDto
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public Guid PatientUserId { get; set; }
        public Guid? DoctorUserId { get; set; } = null;
        public CaseType Type { get; set; }
        public CaseState State { get; set; }

        public List<CaseMessageDto> Messages { get; set; } = new();
        public List<CaseSystemMessageDto> SystemMessages { get; set; } = new();
        public List<PrescriptionDto> Prescriptions { get; set; } = new();
        public List<AppointmentDto> Appointments { get; set; } = new();
    }
}
