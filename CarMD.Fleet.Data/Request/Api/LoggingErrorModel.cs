using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CarMD.Fleet.Data.Request.Api
{
    [DataContract]
    public class LoggingErrorModel
    {
        [DataMember]
        public string Imei { get; set; }

        [DataMember]
        public string ToolFirmware { get; set; }

        [DataMember]
        public string Vin { get; set; }

        [DataMember]
        public int? Mileage { get; set; }

        [DataMember]
        public string Email { get; set; }

        [Required]
        [DataMember]
        public int Type { get; set; }

        [Required]
        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        public string LogData { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
