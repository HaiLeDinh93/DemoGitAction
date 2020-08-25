using CarMD.Fleet.Common.Enum;
using CarMD.Shell.Api.Models;

namespace CarMD.Shell.Api.Helpers
{
    public static class ResponseHelper
    {
        public static MessageBase CreateMessage(string method, string action, ResultCode code, string error = "")
        {
            var message = string.IsNullOrWhiteSpace(error) ? new MessageBase(code) : new MessageBase(code, error);

            message.Version = Globals.ApiVersion;

            message.Method = method;

            message.Action = action;

            return message;
        }
    }
}