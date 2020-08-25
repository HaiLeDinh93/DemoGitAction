using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class MaintenanceGroup
    {
        public MaintenanceGroup()
        {
            MaintenanceMapper = new HashSet<MaintenanceMapper>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ServiceName { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool IsCustomizable { get; set; }

        public virtual ICollection<MaintenanceMapper> MaintenanceMapper { get; set; }
    }
}
