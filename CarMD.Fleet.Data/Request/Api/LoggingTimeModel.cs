using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CarMD.Fleet.Data.Request.Api
{
    [DataContract]
    public class LoggingTimeModel
    {
        [DataMember]
        public long? ReportNumber { get; set; }

        [DataMember]
        public string WorkOrderNumber { get; set; }

        [Required]
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
        public DateTime StartTime { get; set; }

        [Required]
        [DataMember]
        public DateTime EndTime { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
