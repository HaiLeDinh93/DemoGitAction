using MetafuseReference;
using CarMD.Fleet.Core.Mapper;
using CarMD.Fleet.Data.Dto;

namespace CarMD.Fleet.Adapter.Metafuse.Mapper
{
    internal sealed class FixPartOEMMapper : BaseMapper<FixPartOemInfo, FixPartOEM>
    {
        public override void Map(FixPartOemInfo source, FixPartOEM target)
        {
            target.Retailer = source.retailer;
            target.Manufacturer = source.manufacturer;
            target.OemPartNumber = source.oemPartNumber;
        }
    }
}
