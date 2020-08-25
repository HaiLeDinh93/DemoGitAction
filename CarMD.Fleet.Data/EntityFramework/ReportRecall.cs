using CarMD.Fleet.Data.Dto.Metafuse;
using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class ReportRecall
    {
        public Guid Id { get; set; }
        public Guid ReportId { get; set; }
        public string CampaignNumber { get; set; }
        public string DefectConsequence { get; set; }
        public string DefectCorrectiveAction { get; set; }
        public string DefectDescription { get; set; }
        public string RecallDate { get; set; }
        public int RecordNumber { get; set; }

        public virtual Report Report { get; set; }

        public ReportRecall() { }

        public ReportRecall(Recall source)
        {
            Id = Guid.NewGuid();
            CampaignNumber = source.CampaignNumber;
            DefectConsequence = source.DefectConsequence;
            DefectCorrectiveAction = source.DefectCorrectiveAction;
            DefectDescription = source.DefectDescription;
            RecallDate = source.RecallDate;
            RecordNumber = source.RecordNumber;
        }
    }
}
