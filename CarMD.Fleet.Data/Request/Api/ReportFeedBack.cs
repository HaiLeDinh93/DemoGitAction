using System;
using System.Runtime.Serialization;

namespace CarMD.Shell.Api.Models
{
    [DataContract]
    public class ReportFeedBack
    {
        [DataMember]
        public string CustomerEmail { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public bool? LikeThis { get; set; }

        [DataMember]
        public Guid? ReportId { get; set; }
    }
}