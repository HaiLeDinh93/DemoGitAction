using CarMD.Fleet.Data.EntityFramework;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.Dto
{
    public class PrimaryCode
    {
        public PrimaryCode()
        {
            ReportFixes = new List<ReportFix>();
        }

        public string Code { get; set; }
        public PrimaryCodeDetail Detail { get; set; }
        public string FixName { get; set; }
        public List<ReportFix> ReportFixes { get; set; }
    }
}
