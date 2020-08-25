using CarMD.Fleet.Data.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace CarMD.Fleet.Data.Request.Api
{
    [DataContract]
    public class VehicleInnovaDataModel
    {
        [DataMember]
        public string WorkOrderNumber { get; set; }

        [Required]
        [DataMember]
        public string Imei { get; set; }

        [Required]
        [DataMember]
        public int Mileage { get; set; }

        [Required]
        [DataMember]
        public string Vin { get; set; }

        [DataMember]
        public decimal? OriginalTirePressure { get; set; }

        [DataMember]
        public decimal? LFTire { get; set; }

        [DataMember]
        public decimal? RFTire { get; set; }

        [DataMember]
        public decimal? LRTire { get; set; }

        [DataMember]
        public decimal? RRTire { get; set; }

        [DataMember]
        public string TirePressureUnit { get; set; }

        [DataMember]
        public decimal? OriginalBatteryVoltage { get; set; }

        [DataMember]
        public decimal? CurrentBatteryVoltage { get; set; }

        [DataMember]
        public int? BatteryStatus { get; set; }

        [DataMember]
        public int? OilLevel { get; set; }

        [DataMember]
        public int? BrakePadLife { get; set; }

        [DataMember]
        public List<Module> Modules { get; set; }

        [DataMember]
        public bool? IsVinMismatch { get; set; }

        [DataMember]
        public string ToolFirmware { get; set; }

        [DataMember]
        public int? Status { get; set; }

        [DataMember]
        public int? UsbProductId { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string VinProfileRaw { get; set; }

        //monitor
        [DataMember]
        public string MonitorStatusEcmRaw { get; set; }

        [DataMember]
        public string MonitorStatusTcmRaw { get; set; }

        //freezeframe
        [DataMember]
        public string FreezeFrameEcmRaw { get; set; }

        [DataMember]
        public string FreezeFrameTcmRaw { get; set; }

        //dtcs
        [DataMember]
        public string DtcsEcmRaw { get; set; }

        [DataMember]
        public string DtcsTcmRaw { get; set; }

        //vehicle info
        [DataMember]
        public string VehicleInfoEcmRaw { get; set; }

        [DataMember]
        public string VehicleInfoTcmRaw { get; set; }
    }
}
