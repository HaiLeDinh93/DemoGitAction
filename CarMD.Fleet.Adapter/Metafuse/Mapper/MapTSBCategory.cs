using MetafuseReference;
using CarMD.Fleet.Core.Mapper;
using CarMD.Fleet.Data.Dto.Metafuse;

namespace CarMD.Fleet.Adapter.Metafuse.Mapper
{
    internal sealed class MapTSBCategory : BaseMapper<TSBCategoryInfo, TSBCategory>
    {
        public override void Map(TSBCategoryInfo source, TSBCategory target)
        {
            target.Id = source.Id;
            target.Description = source.Description;
            target.TSBCount = source.TSBCount ?? 0;
        }
    }
}
