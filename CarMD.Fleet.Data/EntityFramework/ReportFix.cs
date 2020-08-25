using CarMD.Fleet.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class ReportFix
    {
        public ReportFix()
        {
            ReportFixPart = new HashSet<ReportFixPart>();
        }

        public Guid Id { get; set; }
        public Guid ReportId { get; set; }
        public Guid ReportDtcId { get; set; }
        public Guid? FixNameId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ErrorCode { get; set; }
        public int ErrorCodeType { get; set; }
        public int SortOrder { get; set; }
        public decimal LaborHours { get; set; }
        public decimal LaborRate { get; set; }
        public decimal LaborCost { get; set; }
        public decimal PartsCost { get; set; }
        public decimal AdditionalCost { get; set; }
        public decimal TotalCost { get; set; }
        public int FixRating { get; set; }
        public decimal PredictiveDiagnosticPercentInMileage { get; set; }
        public int PredictiveDiagnosticCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? IsMark { get; set; }

        public virtual Report Report { get; set; }
        public virtual ReportDtc ReportDtc { get; set; }
        public virtual ICollection<ReportFixPart> ReportFixPart { get; set; }


        public ReportFix(Fix source)
        {
            Id = Guid.NewGuid();
            Name = source.Name;
            FixNameId = source.FixNameId;
            Description = source.Description;
            ErrorCode = source.ErrorCode;
            ErrorCodeType = (int)source.CodeType;
            FixRating = source.FixRating;
            LaborCost = source.LaborCost;
            LaborHours = source.LaborHours;
            LaborRate = source.LaborRate;
            PartsCost = source.PartsCost;
            PredictiveDiagnosticCount = source.PredictiveDiagnosticCount;
            PredictiveDiagnosticPercentInMileage = source.PredictiveDiagnosticPercentInMileage;
            AdditionalCost = source.AdditionalCost;
            TotalCost = source.TotalCost;
            SortOrder = source.SortOrder;
            CreatedDate = DateTime.UtcNow;

            if (source.FixParts != null && source.FixParts.Any())
            {
                ReportFixPart = source.FixParts.Select(p => new ReportFixPart(p)).ToList();
            }
        }

    }
}
