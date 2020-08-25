using MetafuseReference;
using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Core.Mapper;
using CarMD.Fleet.Data.Dto.Metafuse;

namespace CarMD.Fleet.Adapter.Metafuse.Mapper
{
    internal sealed class PredictiveDiagnosticMapper : BaseMapper<FixInfo, PredictiveDiagnostic>
    {
        public override void Map(FixInfo source, PredictiveDiagnostic target)
        {

            target.AdditionalCost = source.AdditionalCost;
            target.CodeType = (ErrorCodeSystemType)source.ErrorCodeSystemType;
            target.Name = source.Name ?? string.Empty;
            target.Description = source.Description ?? string.Empty;
            target.ErrorCode = source.ErrorCode ?? string.Empty;
            target.FixNameId = source.FixNameId;


            target.FixRating = source.FixRating;
            target.LaborCost = source.LaborCost;
            target.LaborHours = source.LaborHours;
            target.LaborRate = source.LaborRate;
            target.PartsCost = source.PartsCost;
            target.PredictiveDiagnosticCount = source.PredictiveDiagnosticCount;
            target.PredictiveDiagnosticPercentInMileage = source.PredictiveDiagnosticPercentInMileage;
            target.SortOrder = source.SortOrder;
            target.TotalCost = source.TotalCost;


            if (source.FixParts != null)
            {
                foreach (var fixPart in source.FixParts)
                {
                    target.FixParts.Add(MetafuseMapper.MapFixPart(fixPart));
                }
            }
        }
    }
}
