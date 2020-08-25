using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class ReportScheduledMaintenanceFixPart
    {
        public Guid Id { get; set; }
        public Guid ReportScheduledMaintenanceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ManufacturerName { get; set; }
        public string PartNumber { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public virtual ReportScheduledMaintenance ReportScheduledMaintenance { get; set; }

        public ReportScheduledMaintenanceFixPart() { }
        public ReportScheduledMaintenanceFixPart(Dto.FixPart source)
        {
            Id = Guid.NewGuid();
            Name = source.Name;
            Description = source.Description;
            ManufacturerName = source.ManufacturerName;
            PartNumber = source.PartNumber;
            Price = source.Price;
            Quantity = source.Quantity;
        }
    }
}
