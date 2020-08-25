using CarMD.Fleet.Common.Configuration;
using log4net;
using Mandrill;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CarMD.Fleet.Data.Email;
using System;
using CarMD.Fleet.Common.MandrillApi;
using CarMD.Fleet.Common.Helpers;

namespace CarMD.Fleet.Service.Mandrill
{
    public static class SendMandrillEmailHelper
    {
        private readonly static ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly static string _shell_registration_account_email_template = "shell-registration-account";
        private readonly static string _shell_reset_password_email_template = "shell-reset-account-password";
        private readonly static string _shell_notify_having_dongle = "shell-notify-having-dongle";
        private readonly static string _shell_report_api_email_template = "shell-report-api";
        private readonly static string _shell_confirm_contact_support_email_template = "shell-confirm-contact-support";
        private readonly static string _shell_support_email_template = "shell-contact-support";
        private readonly static string _shell_report_consumer = "shell-report-consumer";

        public static bool SendRegistrationEmail(RegistrationEmailModel user)
        {
            if (user == null || user.Emails == null || !user.Emails.Any()) return false;

            var toList = new List<EmailAddress>();
            foreach (var item in user.Emails)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    toList.Add(new EmailAddress { email = item, name = "" });
                }
            }

            var email = new EmailMessage
            {
                to = toList,
                merge_language = "handlebars",
                subject = user.Subject
            };

            email.AddGlobalVariable("EmailSubject", user.Subject);
            email.AddGlobalVariable("RestLink", user.ResetLogintUrl);
            email.AddGlobalVariable("EmailTemplateImgPath", user.EmailTemplateImgPath);
            email.AddGlobalVariable("EmailSupport", CommonConfiguration.SupportEmails);

            var error = MandrillHelper.SendMessageWithTemplate(email, _shell_registration_account_email_template, null);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _logger.Fatal(error);
                return false;
            }

            return true;
        }

        public static bool SendResetPasswordEmail(ResetPasswordEmailModel user)
        {
            if (user == null || user.Emails == null || !user.Emails.Any()) return false;

            var toList = new List<EmailAddress>();
            foreach (var item in user.Emails)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    toList.Add(new EmailAddress { email = item, name = "" });
                }
            }

            var email = new EmailMessage
            {
                to = toList,
                merge_language = "handlebars",
                subject = user.Subject
            };

            email.AddGlobalVariable("EmailSubject", user.Subject);
            email.AddGlobalVariable("RestLink", user.ResetLogintUrl);

            email.AddGlobalVariable("SupportEmail", CommonConfiguration.ResetPasswordSupportEmails);
            email.AddGlobalVariable("EmailTemplateImgPath", user.EmailTemplateImgPath);
            email.AddGlobalVariable("EmailSupport", CommonConfiguration.SupportEmails);
            var error = MandrillHelper.SendMessageWithTemplate(email, _shell_reset_password_email_template, null);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _logger.Fatal(error);
                return false;
            }

            return true;
        }

        public static string SendNotifyHavingDongle(List<string> emails, string kioskAddress)
        {
            if (emails == null || !emails.Any()) return string.Empty;
            var templateName = _shell_notify_having_dongle;
            var toList = new List<EmailAddress>();
            foreach (var item in emails)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    toList.Add(new EmailAddress { email = item, name = "" });
                }
            }

            var email = new EmailMessage
            {
                to = toList,
                merge_language = "handlebars",
                subject = CommonConfiguration.NotifyHavingDongleEmailSubject,
            };
            email.AddGlobalVariable("EmailSubject", CommonConfiguration.NotifyHavingDongleEmailSubject);
            email.AddGlobalVariable("Copyright", DateTime.UtcNow.Year.ToString());
            email.AddGlobalVariable("EmailTemplateImgPath", UrlHelper.Merge(CommonConfiguration.ShellWebRootUrl, CommonConfiguration.EmailTemplateImgPath));
            email.AddGlobalVariable("EmailSupport", CommonConfiguration.SupportEmails);
            if (!string.IsNullOrEmpty(kioskAddress))
            {
                email.AddGlobalVariable("KioskAddress", kioskAddress);
            }


            var error = MandrillHelper.SendMessageWithTemplate(email, templateName, null);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _logger.Fatal(error);
            }

            return error;
        }

        public static string SendApiReportEmail(ApiReportEmailModel model)
        {
            if (model == null || model.Emails == null || !model.Emails.Any()) return string.Empty;
            var templateName = _shell_report_api_email_template;
            var toList = new List<EmailAddress>();
            foreach (var item in model.Emails)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    toList.Add(new EmailAddress { email = item, name = "" });
                }
            }

            var email = new EmailMessage
            {
                to = toList,
                merge_language = "handlebars",
                subject = model.Subject,
            };

            email.AddGlobalVariable("EmailSubject", model.Subject);
            email.AddGlobalVariable("EmailTemplateImgPath", UrlHelper.Merge(CommonConfiguration.ShellWebRootUrl, CommonConfiguration.EmailTemplateImgPath));
            email.AddGlobalVariable("EmailSupport", CommonConfiguration.SupportEmails);
            email.AddGlobalVariable("Logo", model.Logo);
            email.AddGlobalVariable("ImageLine", model.ImageLine);
            email.AddGlobalVariable("HealthReportUrl", model.HealthReportUrl);
            email.AddGlobalVariable("ShellReportUrl", model.ShellReportUrl);

            var error = MandrillHelper.SendMessageWithTemplate(email, templateName, null);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _logger.Fatal(error);
            }

            return error;
        }

        public static bool SendSupportEmail(SupportEmailModel model)
        {
            if (model == null || model.Emails == null || !model.Emails.Any()) return false;

            string templateName = _shell_support_email_template;

            var toList = new List<EmailAddress>();
            foreach (var item in model.Emails)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    toList.Add(new EmailAddress { email = item, name = "" });
                }
            }
            if (!string.IsNullOrEmpty(CommonConfiguration.SupportCCEmails))
            {
                var ccEmails = CommonConfiguration.SupportCCEmails.Split(';');
                foreach (var item in ccEmails)
                {
                    toList.Add(new EmailAddress { email = item, type = "cc" });
                }
            }
            var email = new EmailMessage
            {
                to = toList,
                merge_language = "handlebars",
                subject = model.Subject,
            };
            email.AddGlobalVariable("EmailSubject", model.Subject);
            email.AddGlobalVariable("EmailTemplateImgPath", model.EmailTemplateImgPath);
            email.AddGlobalVariable("FullName", model.FullName);
            email.AddGlobalVariable("UserEmail", model.UserEmail);
            email.AddGlobalVariable("UserPhone", string.Format("{0} {1}", model.UserPhone, model.Extenstion));
            email.AddGlobalVariable("Message", model.Message);
            email.AddGlobalVariable("TypeSupport", model.Topic);
            email.AddGlobalVariable("RefNumber", model.RefNumber);
            email.AddGlobalVariable("EmailSupport", CommonConfiguration.SupportEmails);

            var error = MandrillHelper.SendMessageWithTemplate(email, templateName, null);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _logger.Fatal(error);
                return false;
            }

            return true;
        }

        public static bool SendConfirmSupportToCustomerEmail(SupportEmailModel model)
        {
            if (model == null || model.Emails == null || !model.Emails.Any()) return false;

            string templateName = _shell_confirm_contact_support_email_template;

            var toList = new List<EmailAddress>();
            foreach (var item in model.Emails)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    toList.Add(new EmailAddress { email = item, name = "" });
                }
            }

            var email = new EmailMessage
            {
                to = toList,
                merge_language = "handlebars",
                subject = model.Subject
            };

            email.AddGlobalVariable("EmailSubject", model.Subject);
            email.AddGlobalVariable("EmailTemplateImgPath", model.EmailTemplateImgPath);
            email.AddGlobalVariable("RefNumber", model.RefNumber);
            email.AddGlobalVariable("EmailSupport", CommonConfiguration.SupportEmails);

            var error = MandrillHelper.SendMessageWithTemplate(email, templateName, null);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _logger.Fatal(error);
                return false;
            }

            return true;
        }

        public static string SendConsumerReportEmail(ReportEmailModel model)
        {
            if (model == null || model.Emails == null || !model.Emails.Any())
            {
                return "Not found any email to send.";
            }

            var templateName = _shell_report_consumer;
            var toList = new List<EmailAddress>();
            foreach (var item in model.Emails)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    toList.Add(new EmailAddress { email = item, name = "" });
                }
            }

            var email = new EmailMessage
            {
                to = toList,
                merge_language = "handlebars",
                subject = model.Subject,
            };

            email.AddGlobalVariable("EmailSubject", model.Subject);
            email.AddGlobalVariable("ImagePath", model.ImagePath);
            email.AddGlobalVariable("ReportUrl", model.ReportUrl);
            email.AddGlobalVariable("VehicleInfo", model.YMM);
            email.AddGlobalVariable("Mileage", model.Mileage);

            email.AddGlobalVariable("CheckEngineLightStatus", model.CheckEngineLightStatus);
            email.AddGlobalVariable("TotalErrorCodeCount", model.TotalErrorCodeCount);

            email.AddGlobalVariable("RepairsCount", model.RepairsCount);
            email.AddGlobalVariable("HasMilFix", model.HasMilFix);

            var error = MandrillHelper.SendMessageWithTemplate(email, templateName, null);
            if (!string.IsNullOrWhiteSpace(error))
            {
                _logger.Fatal(error);
            }

            return error;
        }

    }
}
