using System.Collections.Generic;

namespace CarMD.Fleet.Data.Dto
{
    public class ScheduledMaintenance
    {
        public ScheduledMaintenance()
        {
            Items = new List<string>();
        }

        public int NextMileage { get; set; }
        public int CurrentMileage { get; set; }
        public int TotalItem { get; set; }
        public List<string> Items { get; set; }
        public int ItemCounts
        {
            get
            {
                return Items != null ? Items.Count : 0;
            }
        }
        public int RangeMileage
        {
            get
            {
                return NextMileage - CurrentMileage;
            }
        }
    }
}
