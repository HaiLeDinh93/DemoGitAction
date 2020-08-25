using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Core.Utility;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace CarMD.Shell.Api.Models
{
    [DataContract]
    public class MessageBase
    {
        [DataMember]
        [JsonProperty("code")]
        public ResultCode Code { get; set; }
        [DataMember]
        [JsonProperty("message")]
        public string Message { get; set; }
        [DataMember]
        [JsonProperty("version")]
        public string Version { get; set; }
        [DataMember]
        [JsonProperty("method")]
        public string Method { get; set; }
        [DataMember]
        [JsonProperty("action")]
        public string Action { get; set; }

        public MessageBase(ResultCode messageCode)
        {
            Code = messageCode;
            Message = EnumUtility.GetEnumDescription(messageCode);
        }

        public MessageBase(ResultCode messageCode, string messageDesc)
        {
            Code = messageCode;
            Message = messageDesc;
        }
    }
}