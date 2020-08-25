using System.Collections.Generic;

namespace CarMD.Fleet.Data.Dto
{
    public class PrimaryCodeDetail
    {
        public string LaymansTermDescription { get; set; }
        public string Desc { get; set; }
        public List<string> PossibleCauses { get; set; }
        public string Conditions { get; set; }
        public int LaymansTermsSeverityLevel { get; set; }
        public string LaymansTermsSeverityLevelDefinition { get; set; }
    }
}
