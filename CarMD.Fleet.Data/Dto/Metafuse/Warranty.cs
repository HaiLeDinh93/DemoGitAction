using System;

namespace CarMD.Fleet.Data.Dto.Metafuse
{
    public class Warranty
    {
        public Guid Id { get; set; }
        public Guid ReportId { get; set; }
        public string DescriptionFormatted { get; set; }
        public bool IsTransferable { get; set; }
        public int? MaxMileage { get; set; }
        public int? MaxYears { get; set; }
        public string Notes { get; set; }
        public int WarrantyType { get; set; }
        public string WarrantyTypeDescription { get; set; }
    }
}
