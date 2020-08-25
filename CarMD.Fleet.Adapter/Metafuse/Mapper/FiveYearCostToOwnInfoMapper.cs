using MetafuseReference;
using CarMD.Fleet.Core.Mapper;
using CarMD.Fleet.Data.Dto.Metafuse;

namespace CarMD.Fleet.Adapter.Metafuse.Mapper
{
    internal sealed class FiveYearCostToOwnInfoMapper : BaseMapper<VehicleCostToOwnInfo, FiveYearCostToOwnInfo>
    {
        public override void Map(VehicleCostToOwnInfo source, FiveYearCostToOwnInfo target)
        {
            target.DepreciationCost = source.DepreciationCost;
            target.FuelCost = source.FuelCost;
            target.RepairCost = source.RepairCost;
            target.InsuranceCost = source.InsuranceCost;
            target.MaintenanceCost = source.MaintenanceCost;
            target.TotalCostToOwn = source.TotalCostToOwn;
        }
    }
}
