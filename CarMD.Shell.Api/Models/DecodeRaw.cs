using Innova.Utilities.Shared.Tool;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CarMD.Shell.Api.Models
{
    [DataContract]
    public class DecodeRaw
    {
        [DataMember]
        public string MonitorStatusRaw { get; set; }

        [DataMember]
        public string FreezeFrameRaw { get; set; }

    }

    public class DecodeRawResponse
    {
        public List<MonitorInfo> Monitors { get; set; }

        public List<FreezeFrameInfo> FreezeFrames { get; set; }
    }
}