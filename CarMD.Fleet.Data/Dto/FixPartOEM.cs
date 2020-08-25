using System;

namespace CarMD.Fleet.Data.Dto
{
    public class FixPartOEM
    {
        public Guid Id { get; set; }

        public string Retailer { get; set; }

        public string Manufacturer { get; set; }

        public string OemPartNumber { get; set; }
    }
}
