using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model.Dto
{
    public class CaseDto
    {
        public Guid Id { get; set; }
        public Instant DateTime { get; set; }
        public string Title { get; set; } = null!;
        public Guid PatientUserId { get; set; }
        public Guid? DoctorUserId { get; set; } = null;
        public CaseType Type { get; set; }
        public CaseState State { get; set; }

        public string? Diagnosis { get; set; }

        public List<CaseMessageDto> Messages { get; set; } = new();
        public List<CaseSystemMessageDto> SystemMessages { get; set; } = new();
        public List<PrescriptionDto> Prescriptions { get; set; } = new();
        public List<AppointmentDto> Appointments { get; set; } = new();
        public List<TriageRequestDto> TriageRequests { get; set; } = new();
    }
}
