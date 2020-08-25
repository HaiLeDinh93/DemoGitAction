using System;
using System.Linq;

namespace CarMD.Fleet.Core.Utility.Extensions
{
    public static class TimeZoneExtension
    {
        private static string[] AKSTStates = { "AK" };
        private static string[] HSTStates = { "HI" };
        private static string[] SSTTStates = { "AS" };
        private static string[] CMSTStates = { "GU" };
        private static string[] ATCStates = { "PR", "VI" };
        private static string[] PSTTStates = { "CA", "DC", "WA", "NV", "OR" };
        private static string[] MSTTStates = { "CO", "MT", "NM", "UT", "WY", "AZ", "ID" };
        private static string[] CSTTStates = { "AL", "AR", "IA", "IL", "LA", "MN", "MO", "MS", "OK", "WI", "KS", "KY", "MI", "ND", "SD", "TN", "TX", "NE" };
        private static string[] ESTTStates = { "CT", "DE", "GA", "MA", "MD", "ME", "NC", "NH", "NJ", "NY", "OH", "PA", "RI", "SC", "VA", "VT", "WV", "FL", "IN" };

        /// <summary>
        /// UTC-11: Samoa Standard Time
        /// </summary>
        public static TimeZoneInfo SSTTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Samoa Standard Time");

        /// <summary>
        /// UTC-10: Hawaii-Aleutian Standard Time (HST)
        /// </summary>
        public static TimeZoneInfo HSTTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time");

        /// <summary>
        /// UTC-9: Alaska Standard Time (AKST)
        /// </summary>
        public static TimeZoneInfo AKSTTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Alaskan Standard Time");

        /// <summary>
        /// UTC-8: Pacific Standard Time (PST)
        /// </summary>
        public static TimeZoneInfo PSTTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

        /// <summary>
        /// UTC-7: Mountain Standard Time (MST)
        /// </summary>
        public static TimeZoneInfo MSTTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");

        /// <summary>
        /// UTC-6: Central Standard Time (CST)
        /// </summary>
        public static TimeZoneInfo CSTTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

        /// <summary>
        /// UTC-5: Eastern Standard Time (EST)
        /// </summary>
        public static TimeZoneInfo ESTTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        /// <summary>
        /// UTC-4: Atlantic Standard Time (ATC)
        /// </summary>
        public static TimeZoneInfo ATCTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time");

        /// <summary>
        /// UTC+10: Chamorro Standard Time 
        /// Or: West Pacific Standard Time
        /// </summary>
        public static TimeZoneInfo CMSTTimeZone = TimeZoneInfo.FindSystemTimeZoneById("West Pacific Standard Time");

        /// <summary>
        /// Get Time Zone by State for US
        /// </summary>
        /// <param name="state"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public static TimeZoneInfo GetTimeZoneOfState(string state, string city)
        {
            var uppState = state.ToUpper();
            if (AKSTStates.Any(s => s.Equals(uppState)))
            {
                return AKSTTimeZone;
            }
            if (HSTStates.Any(s => s.Equals(uppState)))
            {
                return HSTTimeZone;
            }
            if (SSTTStates.Any(s => s.Equals(uppState)))
            {
                return SSTTimeZone;
            }
            if (CMSTStates.Any(s => s.Equals(uppState)))
            {
                return CMSTTimeZone;
            }
            if (ATCStates.Any(s => s.Equals(uppState)))
            {
                return ATCTimeZone;
            }
            if (PSTTStates.Any(s => s.Equals(uppState)))
            {
                return PSTTimeZone;
            }
            if (MSTTStates.Any(s => s.Equals(uppState)))
            {
                return MSTTimeZone;
            }
            if (CSTTStates.Any(s => s.Equals(uppState)))
            {
                return CSTTimeZone;
            }
            if (ESTTStates.Any(s => s.Equals(uppState)))
            {
                return ESTTimeZone;
            }
            return null;
        }

    }
}
