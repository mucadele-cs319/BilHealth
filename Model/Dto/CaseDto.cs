using BilHealth.Utility.Enum;

namespace BilHealth.Model.Dto
{
    public record CaseDto
    {
        public Guid Id { get; private set; }
        public DateTime DateTime { get; set; }
        public Guid PatientUserId { get; set; }
        public Guid DoctorUserId { get; set; }
        public CaseType Type { get; set; }
        public CaseState State { get; set; }

        public List<CaseMessage> Messages { get; set; } = new();
        public List<CaseSystemMessage> SystemMessages { get; set; } = new();
        public List<Prescription> Prescriptions { get; set; } = new();
        public List<Appointment> Appointments { get; set; } = new();
    }
}
