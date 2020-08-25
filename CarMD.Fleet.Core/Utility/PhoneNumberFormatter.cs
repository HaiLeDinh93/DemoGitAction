using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Core.Utility
{
    public class PhoneNumberFormatter
    {
        /// <summary>
        /// Convert a string value to 4 phone number format types(1112223333, 111-222-3333, (111)222-3333, (111) 222-3333)
        /// </summary>
        /// <param name="rawString"></param>
        /// <returns></returns>
        public static List<string> GetValues(string rawString)
        {
            if (String.IsNullOrEmpty(rawString))
                return new List<string> { String.Empty, String.Empty, String.Empty, String.Empty };

            var values = new List<string>();
            rawString = rawString.Replace("(", String.Empty)
                .Replace(")", String.Empty)
                .Replace("-", String.Empty)
                .Replace(" ", String.Empty);
            try
            {
                var decValue = decimal.Parse(rawString);
                values.Add(rawString);
                values.Add(String.Format("{0:###-###-####}", decValue));
                values.Add(String.Format("{0:(###)###-####}", decValue));
                values.Add(String.Format("{0:(###) ###-####}", decValue));
            }
            catch
            {
                return new List<string>
                           {
                               rawString,
                               rawString,
                               rawString,
                               rawString
                           };
            }
            return values;
        }

        public static string GetValue(string rawString)
        {
            if (string.IsNullOrEmpty(rawString))
                return string.Empty;

            rawString = rawString.Replace("(", string.Empty)
                .Replace("+", string.Empty)
                .Replace(")", string.Empty)
                .Replace("-", string.Empty)
                .Replace(" ", string.Empty);

            return rawString;
        }
    }
}