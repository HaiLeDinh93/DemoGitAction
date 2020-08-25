using System;

namespace CarMD.Fleet.Core.Utility
{
    public sealed class LogUtility
    {
        public static string GetDetailsErrorMessage(Exception e)
        {
            if (e == null)
                return "";

            string msg = e.StackTrace + "::";

            while (e != null)
            {
                msg += e.Message + "::";
                e = e.InnerException;
            }

            return msg;
        }
    }
}