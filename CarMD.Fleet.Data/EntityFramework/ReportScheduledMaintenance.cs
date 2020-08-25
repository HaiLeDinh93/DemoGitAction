using CarMD.Fleet.Data.Dto.Metafuse;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class ReportScheduledMaintenance
    {
        public ReportScheduledMaintenance()
        {
            ReportScheduledMaintenanceFixPart = new HashSet<ReportScheduledMaintenanceFixPart>();
        }

        public Guid Id { get; set; }
        public Guid ReportId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Cycle { get; set; }
        public int Mileage { get; set; }
        public bool IsCustom { get; set; }
        public int CustomCycle { get; set; }
        public int CustomMonths { get; set; }
        public string FixName { get; set; }
        public Guid? FixNameId { get; set; }
        public string Description { get; set; }
        public string ErrorCode { get; set; }
        public int CodeType { get; set; }
        public decimal LaborCost { get; set; }
        public decimal LaborHours { get; set; }
        public decimal LaborRate { get; set; }
        public decimal PartsCost { get; set; }
        public decimal AdditionalCost { get; set; }
        public decimal TotalCost { get; set; }
        public int PredictiveDiagnosticCount { get; set; }
        public decimal PredictiveDiagnosticPercentInMileage { get; set; }
        public int FixRating { get; set; }
        public int SortOrder { get; set; }
        public bool? IsMark { get; set; }

        public virtual Report Report { get; set; }
        public virtual ICollection<ReportScheduledMaintenanceFixPart> ReportScheduledMaintenanceFixPart { get; set; }

        public ReportScheduledMaintenance(ScheduledMaintenance source)
        {
            Name = source.Name;
            Id = Guid.NewGuid();
            CustomMonths = source.CustomMonths;
            Category = source.Category;
            Cycle = source.Cycle ?? 0;
            Mileage = source.Mileage;
            var fix = source.Fix;
            if (fix != null)
            {
                FixName = fix.Name;
                FixNameId = fix.FixNameId;
                Description = fix.Description;
                ErrorCode = fix.ErrorCode;
                CodeType = (int)fix.CodeType;
                FixRating = fix.FixRating;
                LaborCost = fix.LaborCost;
                LaborHours = fix.LaborHours;
                LaborRate = fix.LaborRate;
                PartsCost = fix.PartsCost;
                PredictiveDiagnosticCount = fix.PredictiveDiagnosticCount;
                PredictiveDiagnosticPercentInMileage = fix.PredictiveDiagnosticPercentInMileage;
                AdditionalCost = fix.AdditionalCost;
                TotalCost = fix.TotalCost;
                SortOrder = fix.SortOrder;

                if (fix.FixParts != null && fix.FixParts.Any())
                {
                    ReportScheduledMaintenanceFixPart = fix.FixParts.Select(v => new ReportScheduledMaintenanceFixPart(v)).ToList();
                }
            }
            IsCustom = source.IsCustom;
        }

    }
}
