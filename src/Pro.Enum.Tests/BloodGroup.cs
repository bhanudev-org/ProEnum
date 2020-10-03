namespace Pro.Enum.Tests
{
    public class BloodGroup : Enumeration
    {
        public static readonly BloodGroup APositive = new BloodGroup(1, "A +ve");
        public static readonly BloodGroup ANegative = new BloodGroup(2, "A -ve");

        public static readonly BloodGroup BPositive = new BloodGroup(3, "B +ve");
        public static readonly BloodGroup BNegative = new BloodGroup(4, "B -ve");

        public static readonly BloodGroup OPositive = new BloodGroup(5, "O +ve");
        public static readonly BloodGroup ONegative = new BloodGroup(6, "O -ve");

        public static readonly BloodGroup ABPositive = new BloodGroup(7, "AB +ve");
        public static readonly BloodGroup ABNegative = new BloodGroup(8, "AB -ve");

        public BloodGroup(int value, string name) : base(value, name) { }
    }
}