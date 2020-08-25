using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CarMD.Fleet.Data.Request.Api
{
    [DataContract]
    public class EraseModuleDtcModel
    {
        [Required]
        [DataMember]
        public Guid ReportId { get; set; }

        [DataMember]
        public Guid? ModuleId { get; set; }

        [DataMember]
        public bool ClearAll { get; set; }
    }
}
