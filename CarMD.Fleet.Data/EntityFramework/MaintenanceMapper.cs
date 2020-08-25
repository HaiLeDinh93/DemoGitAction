using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class MaintenanceMapper
    {
        public long Id { get; set; }
        public string MaintenanceName { get; set; }
        public long MaintenanceGroupId { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        public virtual MaintenanceGroup MaintenanceGroup { get; set; }
    }
}
