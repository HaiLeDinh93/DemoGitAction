using System;


namespace CarMD.Fleet.Data.Dto.Metafuse
{
    public class FiveYearCostToOwnInfo
    {
        public decimal RepairCost { get; set; }

        public decimal MaintenanceCost { get; set; }

        public decimal FuelCost { get; set; }

        public decimal InsuranceCost { get; set; }

        public decimal DepreciationCost { get; set; }

        public decimal TotalCostToOwn { get; set; }
    }
}
