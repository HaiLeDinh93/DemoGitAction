using System.Collections.Generic;

namespace CarMD.Fleet.Data.Dto
{
    public class ModulesStatus
    {
        public int Status { get; set; }
        public int TotalModulesWithIssues { get; set; }
        public int TotalModules { get; set; }
        public int TotalTroubleCodes { get; set; }

        public List<EntityFramework.ReportModule> ModulesHasIssues { get; set; }
    }
}
