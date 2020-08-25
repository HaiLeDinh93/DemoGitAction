using CarMD.Fleet.Data.Dto.Metafuse;
using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class ReportWarranty
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

        public virtual Report Report { get; set; }

        public ReportWarranty() { }

        public ReportWarranty(Warranty source)
        {
            Id = Guid.NewGuid();
            IsTransferable = source.IsTransferable;
            DescriptionFormatted = source.DescriptionFormatted;
            MaxMileage = source.MaxMileage;
            MaxYears = source.MaxYears;
            Notes = source.Notes;
            WarrantyType = source.WarrantyType;
            WarrantyTypeDescription = source.WarrantyTypeDescription;
        }
    }
}
