namespace BilHealth.Utility
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Doctor = "Doctor";
        public const string Staff = "Staff";
        public const string Patient = "Patient";

        public static string[] Names => new string[] { Admin, Doctor, Staff, Patient };
    }
}
