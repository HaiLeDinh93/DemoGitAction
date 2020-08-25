using CarMD.Fleet.Data.Request.Api;
using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class LoggingError : BaseEntity
    {
        public long KioskId { get; set; }
        public Guid UserId { get; set; }
        public string Imei { get; set; }
        public string ToolFirmware { get; set; }
        public string Vin { get; set; }
        public int? Mileage { get; set; }
        public string Email { get; set; }
        public int Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LogData { get; set; }
        public string Description { get; set; }

        public LoggingError() { }

        public LoggingError(LoggingErrorModel model)
        {
            Id = Guid.NewGuid();
            CreatedDate = model.CreatedDate;
            Description = model.Description;
            Email = model.Email;
            Imei = model.Imei;
            LogData = model.LogData;
            Mileage = model.Mileage;
            ToolFirmware = model.ToolFirmware;
            Type = model.Type;
            Vin = model.Vin;
        }
    }
}
