using MetafuseReference;
using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Core.Mapper;
using CarMD.Fleet.Data.Dto;

namespace CarMD.Fleet.Adapter.Metafuse.Mapper
{
    internal sealed class DtcMapper : BaseMapper<ErrorCodeInfo, Dtc>
    {
        public override void Map(ErrorCodeInfo source, Dtc target)
        {
            target.Code = source.Code;
            target.CodeLevel = (ErrorCodeLevel)source.CodeType;
            target.CodeType = (ErrorCodeSystemType)source.ErrorCodeSystemType;
            target.Definitions = MetafuseMapper.MapDtcDefinition(source.ErrorCodeDefinitions);
        }
    }
}
