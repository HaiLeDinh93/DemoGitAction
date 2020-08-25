using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class ReportDtc
    {
        public ReportDtc()
        {
            ReportDtcDefinition = new HashSet<ReportDtcDefinition>();
            ReportFix = new HashSet<ReportFix>();
        }

        public Guid Id { get; set; }
        public Guid ReportId { get; set; }
        public string Code { get; set; }
        public int CodeLevel { get; set; }
        public int CodeType { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }

        public virtual Report Report { get; set; }
        public virtual ICollection<ReportDtcDefinition> ReportDtcDefinition { get; set; }
        public virtual ICollection<ReportFix> ReportFix { get; set; }

        public ReportDtc(Dtc source)
        {
            Id = Guid.NewGuid();
            Code = source.Code;
            CodeLevel = (int)source.CodeLevel;
            CodeType = (int)source.CodeType;

            CreatedDate = DateTime.UtcNow;
            Status = (int)DTCStatus.New;
            ReportFix = new List<ReportFix>();

            if (source.Definitions != null & source.Definitions.Any())
            {
                ReportDtcDefinition = source.Definitions.Select(d => new ReportDtcDefinition(d)).ToList();
            }
        }
    }
}
