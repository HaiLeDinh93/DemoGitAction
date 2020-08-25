using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Core.Utility
{
    public static class AppSettingUtility
    {
        public static IConfiguration Settings { get; set; }
        
        public static string TryGetString(string key, string defaultValue = "")
        {
            return string.IsNullOrWhiteSpace(Settings[key]) ? defaultValue : Settings[key];
        }

        public static int TryGetInt(string key, int defaultValue)
        {
            try
            {
                return int.Parse(Settings[key]);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static long TryGetLong(string key, long defaultValue)
        {
            try
            {
                return long.Parse(Settings[key]);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static decimal TryGetDecimal(string key, decimal defaultValue)
        {
            try
            {
                return decimal.Parse(Settings[key]);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static bool TryGetBoolean(string key, bool defaultValue)
        {
            try
            {
                return bool.Parse(Settings[key]);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static Guid TryGetGuid(string key)
        {
            try
            {
                return new Guid(Settings[key]);
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public static Guid TryGetGuid(string key, Guid value)
        {
            try
            {
                return new Guid(Settings[key]);
            }
            catch
            {
                return value;
            }
        }

        public static IList<int> TryGetListInt(string key)
        {
            var value = Settings[key];
            var result = new List<int>();
            if (!string.IsNullOrEmpty(value))
            {
                var items = value.Split(',');
                if (items.Length > 0)
                {
                    int i;
                    foreach (var s in items)
                    {
                        if (int.TryParse(s.Trim(), out i))
                        {
                            result.Add(i);
                        }
                    }
                }
            }
            return result;
        }
    }
}