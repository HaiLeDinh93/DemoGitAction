using CarMD.Fleet.Common.Configuration;
using CarMD.Fleet.Core.Utility;
using Mandrill;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarMD.Fleet.Common.MandrillApi
{
    public static class MandrillHelper
    {
        public static string SendMessageWithTemplate(EmailMessage emailMessage, string templateName, List<TemplateContent> templateContents, DateTime? send_at = null, bool async = false)
        {
            try
            {
                if (CommonConfiguration.SendEmailInTestEnviroment)
                {
                    templateName = CommonConfiguration.SendEmailInTestEnviromentPrefix + templateName;
                }

                var apiKey = AppSettingUtility.TryGetString("MandrillAPIKey", "z9woSuIMdMqIfIE3DXN3eg");
                var api = new Mandrill.MandrillApi(apiKey);
                var result = api.SendMessage(emailMessage, templateName, templateContents, send_at, async);
                if (result != null && result.Any() && (result[0].Status == EmailResultStatus.Invalid || result[0].Status == EmailResultStatus.Rejected))
                {
                    return string.Format("[SendMandrillMessage] Send email fail - Error: {0} - {1}", result[0].Status, result[0].RejectReason);
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Format("[SendMandrillMessageWithTemplate] Send email fail for template {1} - Error: {0}", LogUtility.GetDetailsErrorMessage(ex), templateName);
            }
        }

        public static string SendMessage(EmailMessage emailMessage, DateTime? send_at = null, bool async = false)
        {
            try
            {
                var apiKey = AppSettingUtility.TryGetString("MandrillAPIKey", "z9woSuIMdMqIfIE3DXN3eg");
                var api = new Mandrill.MandrillApi(apiKey);
                var result = api.SendMessage(emailMessage, send_at, async);
                if (result != null && result.Any() && (result[0].Status == EmailResultStatus.Invalid || result[0].Status == EmailResultStatus.Rejected))
                {
                    return string.Format("[SendMandrillMessage] Send email fail - Error: {0} - {1}", result[0].Status, result[0].RejectReason);
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Format("[SendMandrillMessage] Send email fail - Error: {0}", LogUtility.GetDetailsErrorMessage(ex));
            }
        }
    }
}
