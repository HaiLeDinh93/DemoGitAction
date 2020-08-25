using CarMD.Fleet.Core.Mapper;

namespace CarMD.Fleet.Adapter.Metafuse.Mapper
{
    internal sealed class VehicleMapper : BaseMapper<MetafuseReference.VehicleInfo, Data.Dto.VehicleInfo>
    {
        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        public override void Map(MetafuseReference.VehicleInfo source, Data.Dto.VehicleInfo target)
        {
            if (source == null || !source.IsValid)
            {
                return;
            }
            target.Aaia = source.AAIA;
            target.EngineType = source.EngineType;
            target.IsActive = source.IsValid;
            target.Make = source.Make;
            target.Model = source.Model;
            target.TrimLevel = source.TrimLevel;

            target.Manufacture = source.ManufacturerName;
            if (source.Mileage.HasValue)
            {
                target.Mileage = source.Mileage.Value;
            }
            target.Transmission = source.Transmission;
            target.Vin = source.VIN;
            target.NickName = source.Model;
            target.ModelImageFileUrl = source.ModelImageFileUrl;

            int year;
            if (int.TryParse(source.Year, out year))
            {
                target.Year = year;
            }
        }
    }
}
