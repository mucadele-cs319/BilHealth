namespace BilHealth.Utility.Enum
{
    public enum NotificationType
    {
        CaseNewAppointment, // CaseId, AppointmentId
        CaseAppointmentTimeChanged, // CaseId, AppointmentId
        CaseAppointmentCanceled, // CaseId, AppointmentId
        CaseNewMessage, // CaseId, CaseMessageId
        CaseClosed, // CaseId
        CaseTriaged, // CaseId
        CaseDoctorChanged, // CaseId
        CaseNewPrescription, // CaseId, PrescriptionId
    }
}
