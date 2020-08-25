using MetafuseReference;
using CarMD.Fleet.Core.Mapper;
using CarMD.Fleet.Data.Dto.Metafuse;

using System;

namespace CarMD.Fleet.Adapter.Metafuse.Mapper
{
    internal sealed class ScheduleMaintenanceMapper : BaseMapper<ScheduleMaintenanceServiceInfo, ScheduledMaintenance>
    {
        public override void Map(ScheduleMaintenanceServiceInfo source, ScheduledMaintenance target)
        {
            target.Mileage = source.Mileage;
            target.Name = source.Name;
            //Rule: Adding cyle = mileage
            target.Cycle = source.Mileage;

            target.DateCreated = DateTime.UtcNow;
            target.Category = source.Category;
            target.Fix = source.ServiceInfo != null ? MetafuseMapper.MapFix(source.ServiceInfo) : null;
        }
    }
}
