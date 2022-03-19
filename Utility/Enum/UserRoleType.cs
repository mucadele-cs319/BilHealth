namespace BilHealth.Utility.Enum
{
    public class UserRoleType : StringEnum
    {
        private UserRoleType(string value) : base(value) { }

        public static UserRoleType Admin => new("Admin");
        public static UserRoleType Doctor => new("Doctor");
        public static UserRoleType Staff => new("Staff");
        public static UserRoleType Patient => new("Patient");

        public static UserRoleType[] Names = new[] { Admin, Doctor, Patient, Staff };

        public static class Constant
        {
            public const string Admin = "Admin";
            public const string Doctor = "Doctor";
            public const string Staff = "Staff";
            public const string Patient = "Patient";
        }
    }
}
