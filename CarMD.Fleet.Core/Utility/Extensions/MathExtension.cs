using System;

namespace CarMD.Fleet.Core.Utility.Extensions
{
    public static class MathExtension
    {
        public static decimal TruncateToDecimalPlace(this decimal value, int decimalPlaces = 0)
        {
            if (decimalPlaces <= 0)
                return Math.Truncate(value);
            decimal no = Convert.ToDecimal(Math.Pow(10, decimalPlaces));
            return Math.Truncate(value * no) / no;
        }

        public static double TruncateToDoublePlace(this double value, int doublePlaces = 0)
        {
            if (doublePlaces <= 0)
                return Math.Truncate(value);
            double no = Math.Pow(10, doublePlaces);
            return Math.Truncate(value * no) / no;
        }

        public static decimal CeilingDecimal(this decimal value)
        {
            return (int)Math.Ceiling(TruncateToDecimalPlace(value, 1));
        }
    }
}
