using CarMD.Fleet.Core.Mapper;
using CarMD.Fleet.Data.Dto.Metafuse;
using MetafuseReference;

namespace CarMD.Fleet.Adapter.Metafuse.Mapper
{
    internal sealed class WarrantyMapper : BaseMapper<VehicleWarrantyDetailInfo, Warranty>
    {
        public override void Map(VehicleWarrantyDetailInfo source, Warranty target)
        {
            target.DescriptionFormatted = source.DescriptionFormatted;
            target.MaxMileage = source.MaxMileage;
            target.MaxYears = source.MaxYears;
            target.Notes = source.Notes;
            target.WarrantyType = source.WarrantyType;
            target.WarrantyTypeDescription = source.WarrantyTypeDescription;
            target.IsTransferable = source.IsTransferable;
        }
    }
}
