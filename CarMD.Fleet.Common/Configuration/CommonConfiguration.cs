using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Core.Utility;

namespace CarMD.Fleet.Common.Configuration
{
    public class CommonConfiguration
    {
        public static string StaticUrl { get; private set; }

        public static string StaticRootPath { get; private set; }

        public static string EmailSenderAddress { get; private set; }

        public static string EmailSenderName { get; private set; }

        public static string EmailSource { get; private set; }

        public static int EmailTotalRetryTime { get; private set; }

        public static string EncryptKey { get; }

        public static string EmailTemplateImgPath { get; private set; }

        public static string ReportEmailTemplateImgPath { get; private set; }

        public static string MonitorFileUrl { get; private set; }

        public static string NotifyHavingDongleEmailSubject { get; private set; }

        #region Impersonate User

        public static string ShellWebRootUrl { get; private set; }
        public static string ShellCsrRootUrl { get; private set; }
        public static string ShellConsumerRootUrl { get; private set; }

        #endregion

        #region Metafuse

        public static string MetafuseWebServiceKey { get; private set; }

        public static string MetafuseServiceUser { get; private set; }

        #endregion Metafuse

        #region Twilio

        public static string TwilioPhoneNumber { get; private set; }

        public static string TwilioAccountSid { get; private set; }

        public static string TwilioAuthToken { get; private set; }

        public static bool TwilioEnabled { get; private set; }

        #endregion

        #region Mandrill

        public static string MandrillAPIKey { get; private set; }

        public static bool SendEmailInTestEnviroment { get; private set; }

        public static string SendEmailInTestEnviromentPrefix { get; private set; }

        #endregion

        #region Email Titltes

        public static string RegistrationEmailSubject { get; private set; }

        public static string ResetPasswordEmailSubject { get; private set; }

        public static string SupportEmailSubject { get; private set; }

        public static string SupportEmails { get; private set; }

        public static string ResetPasswordSupportEmails { get; private set; }

        public static string SupportCCEmails { get; private set; }

        #endregion Email Titltes

        public static int TicketInterval { get; private set; }

        public static string KioskLogoFolder { get; private set; }

        #region Auth0

        public static string Auth0Domain { get; private set; }

        public static string Auth0ClientId { get; private set; }

        public static string Auth0ClientSecret { get; private set; }

        public static string Auth0RedirectUri { get; private set; }

        #endregion

        static CommonConfiguration()
        {
            StaticUrl = AppSettingUtility.TryGetString("StaticUrl", "http://static.vehimatics.com");

            StaticRootPath = AppSettingUtility.TryGetString("StaticRootPath", "D:\\Static");

            EmailSenderAddress = AppSettingUtility.TryGetString("EmailSenderAddress", "support@carmd.com");

            EmailSenderName = AppSettingUtility.TryGetString("EmailSenderName", "FleetMD");

            EmailSource = AppSettingUtility.TryGetString("EmailSource", "FleetMD");

            EmailTotalRetryTime = AppSettingUtility.TryGetInt("EmailTotalRetryTime", 3);

            MetafuseWebServiceKey = AppSettingUtility.TryGetString("MetafuseWebServiceKey", "YcW2EnpI/r7yOkiuVkHWIY49kE5rlud1KDUOCeP4eOsjAa1YY6dCRPih0SYayshz");

            MetafuseServiceUser = AppSettingUtility.TryGetString("MetafuseServiceUser", "0E022A93-E768-4A50-AE43-F3C7C9DE21FD");

            SupportEmails = AppSettingUtility.TryGetString("SupportEmails", "itssuport@innova.com");

            SupportCCEmails = AppSettingUtility.TryGetString("SupportCCEmails", string.Empty);

            ResetPasswordSupportEmails = AppSettingUtility.TryGetString("ResetPasswordSupportEmails", "itssuport@innova.com");

            #region Twilio SMS Service

            TwilioPhoneNumber = AppSettingUtility.TryGetString("TwilioPhoneNumber", "+15005550006");
            TwilioAccountSid = AppSettingUtility.TryGetString("TwilioAccountSid", "AC04c8ba11649b8fa2a6a9c601d7b87111");
            TwilioAuthToken = AppSettingUtility.TryGetString("TwilioAuthToken", "c0f1aa1bd6d72e055c5a4f6c845a7f42");
            TwilioEnabled = AppSettingUtility.TryGetBoolean("TwilioEnabled", false);

            #endregion

            EncryptKey = AppSettingUtility.TryGetString("EncryptKey", "ProScan2018");

            SupportEmailSubject = AppSettingUtility.TryGetString("SupportEmailSubject", "{0}");

            MandrillAPIKey = AppSettingUtility.TryGetString("MandrillAPIKey", "z9woSuIMdMqIfIE3DXN3eg");

            SendEmailInTestEnviroment = AppSettingUtility.TryGetBoolean("SendEmailInTestEnviroment", false);

            SendEmailInTestEnviromentPrefix = AppSettingUtility.TryGetString("SendEmailInTestEnviromentPrefix", "Testing_");

            RegistrationEmailSubject = AppSettingUtility.TryGetString("RegistrationEmailSubject", "Complete Your ITS Account Registration");

            ResetPasswordEmailSubject = AppSettingUtility.TryGetString("ResetPasswordEmailSubject", "Innova Telematics Solutions Password Reset");

            EmailTemplateImgPath = AppSettingUtility.TryGetString("EmailTemplateImgPath", "/Content/shell_web/img/Email_Template/");

            ReportEmailTemplateImgPath = AppSettingUtility.TryGetString("ReportEmailTemplateImgPath", "/Content/themes/img/report/");

            MonitorFileUrl = AppSettingUtility.TryGetString("MonitorFileUrl", "http://resources.innova.com/ErrorCodeInfo/");

            NotifyHavingDongleEmailSubject = AppSettingUtility.TryGetString("NotifyHavingDongleEmailSubject", "Notify Having Dongle");

            TicketInterval = AppSettingUtility.TryGetInt("TicketInterval", 60);

            KioskLogoFolder = AppSettingUtility.TryGetString("KioskLogoPath", "KioskLogo\\");

            #region Impersonate User

            ShellWebRootUrl = AppSettingUtility.TryGetString("ShellWebRootUrl", "https://dev2-shell.carmd.com/member/");
            ShellCsrRootUrl = AppSettingUtility.TryGetString("ShellCsrRootUrl", "https://dev2-shell.carmd.com/admin/");
            ShellConsumerRootUrl = AppSettingUtility.TryGetString("ShellConsumerRootUrl", "https://dev2-shell.carmd.com/customer/");

            #endregion Impersonate User

            Auth0Domain = AppSettingUtility.TryGetString("auth0:Domain");

            Auth0ClientId = AppSettingUtility.TryGetString("auth0:ClientId");

            Auth0ClientSecret = AppSettingUtility.TryGetString("auth0:ClientSecret");

            Auth0RedirectUri = AppSettingUtility.TryGetString("auth0:RedirectUri");
        }
    }
}
