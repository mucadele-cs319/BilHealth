using System.ComponentModel.DataAnnotations;
using BilHealth.Utility.Enum;

namespace BilHealth.Model
{
    public class Case
    {
        [Required] public Guid Id { get; private set; }
        [Required] public DateTime DateTime { get; set; }
        [Required] public Guid PatientUserId { get; set; }
        public User PatientUser { get; set; } = null!;
        public Guid? DoctorUserId { get; set; }
        public User? DoctorUser { get; set; }
        public Guid? NurseUserId { get; set; }
        public User? NurseUser { get; set; }
        [Required] public CaseType Type { get; set; }
        [Required] public CaseState State { get; set; } = CaseState.Open;

        public List<CaseMessage>? Messages { get; set; }
        public List<CaseSystemMessage>? SystemMessages { get; set; }
        public List<Prescription>? Prescriptions { get; set; }
        public List<Appointment>? Appointments { get; set; }
    }
}
