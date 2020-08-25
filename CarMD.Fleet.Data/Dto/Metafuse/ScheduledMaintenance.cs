using System;

namespace CarMD.Fleet.Data.Dto.Metafuse
{
    public class ScheduledMaintenance
    {
        public Guid Id { get; set; }

        public Guid ReportId { get; set; }

        public string Name { get; set; }

        public int? Cycle { get; set; }

        public DateTime DateCreated { get; set; }

        public int CustomMonths { get; set; }

        public int CustomCycle { get; set; }

        public int Mileage { get; set; }

        public string Category { get; set; }

        public bool IsCustom { get; set; }

        public Fix Fix { get; set; }
    }
}
