namespace BilHealth.Utility.Enum
{
    public enum NotificationType
    {
        /// <summary>
        ///     <para>
        ///         Notifies when there is a new appointment on a case.
        ///     </para>
        ///     <code>
        ///         referenceId1 = CaseId,
        ///         referenceId2 = AppointmentId
        ///     </code>
        /// </summary>
        CaseNewAppointment,
        /// <summary>
        ///     <para>
        ///         Notifies when the active appointment on a case has its time changed.
        ///     </para>
        ///     <code>
        ///         referenceId1 = CaseId,
        ///         referenceId2 = AppointmentId
        ///     </code>
        /// </summary>
        CaseAppointmentTimeChanged,
        /// <summary>
        ///     <para>
        ///         Notifies when the active appointment on a case is canceled.
        ///     </para>
        ///     <code>
        ///         referenceId1 = CaseId,
        ///         referenceId2 = AppointmentId
        ///     </code>
        /// </summary>
        CaseAppointmentCanceled,
        /// <summary>
        ///     <para>
        ///         Notifies when there is a new message on a case.
        ///     </para>
        ///     <code>
        ///         referenceId1 = CaseId,
        ///         referenceId2 = CaseMessageId
        ///     </code>
        /// </summary>
        CaseNewMessage,
        /// <summary>
        ///     <para>
        ///         Notifies when a case is closed.
        ///     </para>
        ///     <code>
        ///         referenceId1 = CaseId,
        ///         referenceId2 = null
        ///     </code>
        /// </summary>
        CaseClosed,
        /// <summary>
        ///     <para>
        ///         Notifies when the triage request on a case is approved.
        ///     </para>
        ///     <code>
        ///         referenceId1 = CaseId,
        ///         referenceId2 = null
        ///     </code>
        /// </summary>
        CaseTriaged,
        /// <summary>
        ///     <para>
        ///         Notifies when a case has its doctor resign from the case.
        ///     </para>
        ///     <code>
        ///         referenceId1 = CaseId,
        ///         referenceId2 = null
        ///     </code>
        /// </summary>
        CaseDoctorResigned,
        /// <summary>
        ///     <para>
        ///         Notifies when there is a new prescription on a case.
        ///     </para>
        ///     <code>
        ///         referenceId1 = CaseId,
        ///         referenceId2 = PrescriptionId
        ///     </code>
        /// </summary>
        CaseNewPrescription,
        /// <summary>
        ///     <para>
        ///         Notifies when a patient has a new test result uploaded.
        ///     </para>
        ///     <code>
        ///         referenceId1 = TestResultId,
        ///         referenceId2 = null
        ///     </code>
        /// </summary>
        TestResultNew
    }
}
