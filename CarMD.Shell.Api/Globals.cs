using CarMD.Fleet.Core.Utility;

namespace CarMD.Shell.Api
{
    public static class Globals
    {
        public static string ApiVersion
        {
            get { return AppSettingUtility.TryGetString("ApiVersion", "1.0.0"); }
        }

        public static string ApiKey
        {
            get { return AppSettingUtility.TryGetString("ApiKey", "5BCB8852-1575-4AD4-905C-18094CBD3DDA"); }
        }

        public static string FeedbackEmails
        {
            get { return AppSettingUtility.TryGetString("FeedbackEmails", "niko.ruiz@carmd.com;jlackey@transprt.io"); }
        }

        public static string MissingDongleEmails
        {
            get { return AppSettingUtility.TryGetString("MissingDongleEmails", "Eugene.Reyes@vehicleiq.co;Ed.komo@vehicleiq.co;James.tunley@vehicleiq.co;Shariq.minhas@vehicleiq.co"); }
        }

        public static string LockboxEmails
        {
            get { return AppSettingUtility.TryGetString("LockboxEmails", "niko.ruiz@carmd.com;jlackey@transprt.io"); }
        }
    }
}
