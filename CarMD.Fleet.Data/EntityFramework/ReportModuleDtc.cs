using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class ReportModuleDtc
    {
        public Guid Id { get; set; }
        public Guid ReportModuleId { get; set; }
        public string Code { get; set; }
        public string Def { get; set; }
        public string Type { get; set; }

        public virtual ReportModule ReportModule { get; set; }
    }
}
