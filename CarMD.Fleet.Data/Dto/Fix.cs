using CarMD.Fleet.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMD.Fleet.Data.Dto
{
    public class Fix
    {
        public Fix()
        {
            FixParts = new List<FixPart>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? FixNameId { get; set; }
        public string ErrorCode { get; set; }
        public int FixRating { get; set; }
        public decimal LaborCost { get; set; }
        public decimal LaborHours { get; set; }
        public decimal LaborRate { get; set; }
        public decimal PartsCost { get; set; }
        public decimal PredictiveDiagnosticPercentInMileage { get; set; }
        public decimal AdditionalCost { get; set; }
        public decimal TotalCost { get; set; }
        public int PredictiveDiagnosticCount { get; set; }

        public ErrorCodeSystemType CodeType { get; set; }
        public int SortOrder { get; set; }

        public IList<FixPart> FixParts { get; set; }
    }
}
