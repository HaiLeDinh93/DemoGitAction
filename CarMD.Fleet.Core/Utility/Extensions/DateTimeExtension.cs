namespace CarMD.Fleet.Core.Utility.Extensions
{
    using System;

    public static class DateTimeExtension
    {
        private static TimeZoneInfo PSTTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
        private static TimeZoneInfo ESTTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        public static DateTime PSTToUTC(this DateTime pst)
        {
            return TimeZoneInfo.ConvertTimeToUtc(pst, PSTTimeZone);
        }

        public static DateTime UTCToPST(this DateTime utc)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utc, PSTTimeZone);
        }

        public static DateTime UTCToEST(this DateTime utc)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utc, ESTTimeZone);
        }

        public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes, int seconds, int milliseconds)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                hours,
                minutes,
                seconds,
                milliseconds);
        }

        public static DateTime LastTimeOfDay(this DateTime dateTime)
        {
            return dateTime.ChangeTime(23, 59, 59, 0);
        }

        public static DateTime ConvertToTimeZone(this DateTime utc, string timeZoneInfoId)
        {
            if ("UTC".Equals(timeZoneInfoId))
            {
                return utc;
            }
            else if (string.IsNullOrWhiteSpace(timeZoneInfoId))
            {
                timeZoneInfoId = "Pacific Standard Time";
            }

            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneInfoId);
            return TimeZoneInfo.ConvertTimeFromUtc(utc, timeZone);
        }

        public static DateTime ConvertToUTC(this DateTime datetime, string timeZoneInfoId)
        {
            if ("UTC".Equals(timeZoneInfoId))
            {
                return datetime;
            }
            else if (string.IsNullOrWhiteSpace(timeZoneInfoId))
            {
                timeZoneInfoId = "Pacific Standard Time";
            }

            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneInfoId);
            return TimeZoneInfo.ConvertTimeToUtc(datetime, timeZone);
        }

        public static string GetTimeAgo(this DateTime datetime, string timeZoneInfoId = "")
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var now = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(timeZoneInfoId))
            {
                now = now.ConvertToTimeZone(timeZoneInfoId);
            }

            var ts = new TimeSpan(now.Ticks - datetime.Ticks);

            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            return dt.StartOfWeek(startOfWeek).AddDays(6).Date;
        }

        public static DateTime PSTToStateDatetime(this DateTime localTime, string state, string city)
        {
            var timeZone = TimeZoneExtension.GetTimeZoneOfState(state, city);
            return localTime.PSTToTimeZone(timeZone);
        }

        public static DateTime PSTToTimeZone(this DateTime localTime, TimeZoneInfo timeZoneInfo)
        {
            var stateTime = localTime;
            if (timeZoneInfo == null)
            {
                return stateTime;
            }

            if (localTime.Kind == DateTimeKind.Local)
            {
                stateTime = TimeZoneInfo.ConvertTime(localTime, TimeZoneInfo.Local, TimeZoneExtension.PSTTimeZone);
            }
            return TimeZoneInfo.ConvertTime(stateTime, TimeZoneExtension.PSTTimeZone, timeZoneInfo);
        }
    }
}