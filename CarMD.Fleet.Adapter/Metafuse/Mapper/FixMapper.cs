using MetafuseReference;
using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Core.Mapper;
using CarMD.Fleet.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMD.Fleet.Adapter.Metafuse.Mapper
{
    internal sealed class FixMapper : BaseMapper<FixInfo, Fix>
    {
        public override void Map(FixInfo source, Fix target)
        {
            target.AdditionalCost = source.AdditionalCost;
            target.CodeType = (ErrorCodeSystemType)source.ErrorCodeSystemType;
            target.Name = source.Name ?? string.Empty;
            target.Description = source.Description ?? string.Empty;
            target.ErrorCode = source.ErrorCode ?? string.Empty;
            target.FixNameId = source.FixNameId;

            if (source.FixParts != null)
            {
                foreach (var fixPart in source.FixParts)
                {
                    target.FixParts.Add(MetafuseMapper.MapFixPart(fixPart));
                }
            }
            target.FixRating = source.FixRating;
            target.LaborCost = source.LaborCost;
            target.LaborHours = source.LaborHours;
            target.LaborRate = source.LaborRate;
            target.PartsCost = source.PartsCost;
            target.PredictiveDiagnosticCount = source.PredictiveDiagnosticCount;
            target.PredictiveDiagnosticPercentInMileage = source.PredictiveDiagnosticPercentInMileage;
            target.SortOrder = source.SortOrder;
            target.TotalCost = source.TotalCost;
        }
    }
}
