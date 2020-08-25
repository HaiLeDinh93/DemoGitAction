using CarMD.Fleet.Data.Dto;
using System;

namespace CarMD.Fleet.Data.Request.Api
{
    public class CreateReportRequest
    {
        public Guid UserId { get; set; }

        public long KioskId { get; set; }

        public VehicleDataModel VehicleDataModel { get; set; }

        public VehicleInfo Vehicle { get; set; }

        public string MilDTC { get; set; }

        public int ToolMilStatus { get; set; }

        public string TimeZone { get; set; }

        public string RawString { get; set; }
    }
}
