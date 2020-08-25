using CarMD.Fleet.Data.Request.Api;
using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class LoggingTime: BaseEntity
    {
        public long KioskId { get; set; }
        public Guid UserId { get; set; }
        public long? ReportNumber { get; set; }
        public string WorkOrderNumber { get; set; }
        public string Imei { get; set; }
        public string ToolFirmware { get; set; }
        public string Vin { get; set; }
        public int? Mileage { get; set; }
        public string Email { get; set; }
        public int Type { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }

        public LoggingTime() { }

        public LoggingTime(LoggingTimeModel model)
        {
            Id = Guid.NewGuid();
            WorkOrderNumber = model.WorkOrderNumber;
            Vin = model.Vin;
            Type = model.Type;
            ToolFirmware = model.ToolFirmware;
            StartTime = model.StartTime;
            ReportNumber = model.ReportNumber;
            Mileage = model.Mileage;
            Imei = model.Imei;
            EndTime = model.EndTime;
            Email = model.Email;
            Description = model.Description;
        }
    }
}
