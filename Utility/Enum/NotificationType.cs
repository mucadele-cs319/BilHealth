namespace BilHealth.Utility.Enum
{
    public enum NotificationType
    {
        CaseNewAppointment, // CaseId, AppointmentId
        CaseAppointmentTimeChanged, // CaseId, AppointmentId
        CaseNew, // CaseId
        CaseNewMessage, // CaseId, CaseMessageId
        CaseClosed, // CaseId
        CaseDoctorChanged, // CaseId
        CaseNewPrescription, // CaseId, PrescriptionId
        CaseNewTestEmbed, // CaseId, EmbedId
        CaseNewCaseEmbed // CaseId, EmbedId
    }
}
