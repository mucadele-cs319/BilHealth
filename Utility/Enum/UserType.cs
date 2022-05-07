namespace BilHealth.Utility.Enum
{
    public static class UserType
    {
        public const string Admin = nameof(Model.Admin);
        public const string Doctor = nameof(Model.Doctor);
        public const string Nurse = nameof(Model.Nurse);
        public const string Staff = nameof(Model.Staff);
        public const string Patient = nameof(Model.Patient);

        public static readonly string[] Names = new[] { Admin, Doctor, Nurse, Staff, Patient };
    }
}
