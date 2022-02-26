using System.ComponentModel.DataAnnotations;
using BilHealth.Utility.Enum;

namespace BilHealth.Model
{
    public class Case
    {
        [Required] public Guid Id { get; private set; }
        [Required] public Guid PatientUserId { get; set; }
        [Required] public Guid DoctorUserId { get; set; }
        [Required] public CaseType Type { get; set; }

        public List<CaseMessage>? Messages { get; set; }
        public List<CaseSystemMessage>? SystemMessages { get; set; }
        public List<Prescription>? Prescriptions { get; set; }
        public List<Appointment>? Appointments { get; set; }
    }
}
