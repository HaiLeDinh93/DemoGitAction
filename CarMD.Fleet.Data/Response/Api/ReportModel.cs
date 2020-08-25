using CarMD.Fleet.Data.Dto;
using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.Response.Api
{
    public class ReportModel
    {
        public Guid Id { get; set; }

        public long ReportNumber { get; set; }

        public string WorkOrderNumber { get; set; }

        public string CreatedDate { get; set; }

        public string CreatedTime { get; set; }

        public string Vin { get; set; }

        public string YMM { get; set; }

        public string YMME { get; set; }

        public string Engine { get; set; }

        public int Mileage { get; set; }

        public string TimeAgo { get; set; }

        public string PrimaryErrorCode { get; set; }

        public int ModulesScannedCount { get; set; }

        public int TotalErrorCodeCount { get; set; }

        public int? OilLevel { get; set; }

        public int? BrakePadLife { get; set; }

        public int? BatteryStatus { get; set; }

        public decimal? OriginalTirePressure { get; set; }

        public decimal? LFTire { get; set; }

        public decimal? RFTire { get; set; }

        public decimal? LRTire { get; set; }

        public decimal? RRTire { get; set; }

        public string TirePressureUnit { get; set; }

        public int TotalDTCCount { get; set; }

        public List<ReportModule> ModulesWithDTC { get; set; }

        public List<ReportModule> ModulesWithoutDTC { get; set; }

        public int? Status { get; set; }

        public int CheckEngineLightStatus { get; set; }

        public int UrgencyOfRepairStatus { get; set; }

        public int BatteryVoltageStatus { get; set; }
    }
}
