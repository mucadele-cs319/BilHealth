namespace BilHealth.Utility.Enum
{
    public class UserRoleType : StringEnum
    {
        private UserRoleType(string value) : base(value) { }

        public static UserRoleType Admin => new("Admin");
        public static UserRoleType Doctor => new("Doctor");
        public static UserRoleType Nurse => new("Nurse");
        public static UserRoleType Staff => new("Staff");
        public static UserRoleType Patient => new("Patient");

        public static UserRoleType[] Names = new[] { Admin, Doctor, Nurse, Patient, Staff };

        public static class Constant
        {
            public const string Admin = "Admin";
            public const string Doctor = "Doctor";
            public const string Nurse = "Nurse";
            public const string Staff = "Staff";
            public const string Patient = "Patient";
        }

        public override bool Equals(object? obj)
        {
            return obj is UserRoleType type &&
                   Value == type.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        public static bool operator ==(UserRoleType type1, UserRoleType type2) {
            if (ReferenceEquals(type1, null) || ReferenceEquals(type2, null)) return false;
            return type1.Equals(type2);
        }

        public static bool operator !=(UserRoleType type1, UserRoleType type2) => !(type1 == type2);
    }
}
