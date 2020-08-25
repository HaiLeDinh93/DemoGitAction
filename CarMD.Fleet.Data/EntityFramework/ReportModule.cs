using System;
using System.Collections.Generic;
using System.Linq;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class ReportModule
    {
        public ReportModule()
        {
            ReportModuleDtc = new HashSet<ReportModuleDtc>();
        }

        public Guid Id { get; set; }
        public Guid ReportId { get; set; }
        public string ModuleName { get; set; }
        public string SubSystem { get; set; }
        public string System { get; set; }
        public int? InnovaGroup { get; set; }
        public bool? IsMark { get; set; }

        public virtual Report Report { get; set; }
        public virtual ICollection<ReportModuleDtc> ReportModuleDtc { get; set; }

        public List<ReportModuleDtc> GetReportModuleDtcs(string primaryCode)
        {
            primaryCode = string.IsNullOrEmpty(primaryCode) ? "" : primaryCode.Trim();
            var moduleDTCs = new List<ReportModuleDtc>();
            if (ReportModuleDtc == null)
            {
                return moduleDTCs;
            }
            var dtcs = ReportModuleDtc.Where(dtc => !dtc.Code.Equals(primaryCode, StringComparison.OrdinalIgnoreCase));

            foreach (var dtc in dtcs)
            {
                if (!string.IsNullOrWhiteSpace(dtc.Code) && !moduleDTCs.Any(v => dtc.Code.Equals(v.Code, StringComparison.OrdinalIgnoreCase)))
                {
                    moduleDTCs.Add(dtc);
                }
            }
            return moduleDTCs;
        }
    }
}
