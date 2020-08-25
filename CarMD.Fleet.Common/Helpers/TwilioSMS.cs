using CarMD.Fleet.Common.Configuration;
using CarMD.Fleet.Core.Utility;
using System;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace CarMD.Fleet.Common.Helper
{
    public static class TwilioSMS
    {
        static TwilioSMS()
        {
            TwilioClient.Init(CommonConfiguration.TwilioAccountSid, CommonConfiguration.TwilioAuthToken);
        }

        public static string Send(string to, string body)
        {         
            var message = string.Empty;
            try
            {
                MessageResource sms = MessageResource.Create(
                    to: new PhoneNumber(to),
                    from: new PhoneNumber(CommonConfiguration.TwilioPhoneNumber),
                    body: body);

                if (!string.IsNullOrWhiteSpace(sms.ErrorMessage))
                {
                    message = string.Format("Twilio: SMS delivery failure from {0} - to: {1} - {2} - {3}", CommonConfiguration.TwilioPhoneNumber, to, sms.ErrorCode, sms.ErrorMessage);
                }
                else
                {
                    System.Threading.Thread.Sleep(3000);
                    sms = MessageResource.Fetch(sms.Sid);
                    if (!string.IsNullOrWhiteSpace(sms.ErrorMessage))
                    {
                        message = string.Format("Twilio: SMS delivery failure from {0} - to: {1} - {2} - {3}", CommonConfiguration.TwilioPhoneNumber, to, sms.ErrorCode, sms.ErrorMessage);
                    }
                }
            }
            catch (ApiException ex)
            {
                message = "Twilio: api error - " + ex.Message;
            }
            catch (Exception ex)
            {
                message = "Twilio: error - " + LogUtility.GetDetailsErrorMessage(ex);
            }
            return message;
        }
    }
}
