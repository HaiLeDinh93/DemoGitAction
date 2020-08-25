using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class ReportUnScheduledMaintenanceFixPart
    {
        public Guid Id { get; set; }
        public Guid ReportUnScheduledMaintenanceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ManufacturerName { get; set; }
        public string PartNumber { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public virtual ReportUnScheduledMaintenance ReportUnScheduledMaintenance { get; set; }

        public ReportUnScheduledMaintenanceFixPart() { }

        public ReportUnScheduledMaintenanceFixPart(Dto.FixPart source)
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
