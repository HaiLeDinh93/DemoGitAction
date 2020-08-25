using MetafuseReference;
using CarMD.Fleet.Core.Mapper;
using CarMD.Fleet.Data.Dto.Metafuse;

namespace CarMD.Fleet.Adapter.Metafuse.Mapper
{
    internal sealed class RecallMapper : BaseMapper<RecallInfo, Recall>
    {
        public override void Map(RecallInfo source, Recall target)
        {
            target.CampaignNumber = source.CampaignNumber;
            target.DefectConsequence = source.DefectConsequence;
            target.DefectCorrectiveAction = source.DefectCorrectiveAction;
            target.DefectDescription = source.DefectDescription;
            target.RecallDate = source.RecallDateString;
            target.RecordNumber = source.RecordNumber;
        }
    }
}
