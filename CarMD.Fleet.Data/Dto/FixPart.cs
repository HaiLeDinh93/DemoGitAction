using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.Dto
{
    public class FixPart
    {
        public FixPart()
        {
            FixPartOEMs = new List<FixPartOEM>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ManufacturerName { get; set; }

        public string PartNumber { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string PartOEMManufacturers { get; set; }

        public IList<FixPartOEM> FixPartOEMs { get; set; }

        public decimal SubTotal { get { return Price * Quantity; } }
    }
}
