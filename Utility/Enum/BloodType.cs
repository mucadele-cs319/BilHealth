namespace BilHealth.Utility.Enum
{
    public enum BloodType
    {
        Unspecified,
        APos,
        ANeg,
        BPos,
        BNeg,
        ABPos,
        ABNeg,
        OPos,
        ONeg
    }

    public static class BloodTypeExtensions
    {
        public static string Stringify(this BloodType bloodType) => bloodType switch
        {
            BloodType.APos => "A Rh+",
            BloodType.ANeg => "A Rh-",
            BloodType.BPos => "B Rh+",
            BloodType.BNeg => "B Rh-",
            BloodType.ABPos => "AB Rh+",
            BloodType.ABNeg => "AB Rh-",
            BloodType.OPos => "O Rh+",
            BloodType.ONeg => "O Rh-",
            _ => throw new ArgumentOutOfRangeException(nameof(bloodType), $"Unexpected value: {bloodType}")
        };
    }
}
