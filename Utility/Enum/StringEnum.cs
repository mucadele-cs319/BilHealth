namespace BilHealth.Utility.Enum
{
    public abstract class StringEnum
    {
        protected StringEnum(string value)
        {
            Value = value;
        }

        protected string Value;

        public override string ToString() => Value;
        public static implicit operator String(StringEnum s) => s.ToString();
    }
}
