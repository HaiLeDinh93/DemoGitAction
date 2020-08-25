using System;

namespace CarMD.Fleet.Data.Dto
{
    public class ReportHistory
    {
        public Guid Id { get; set; }
        public long ReportNumber { get; set; }
        public string WorkOrderNumber { get; set; }
        public string CreatedDateTimeUtc { get; set; }
        public string CreatedDateTime { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
        public string Vin { get; set; }
        public string YMM { get; set; }
        public string YMME { get; set; }
        public string Engine { get; set; }
        public int Mileage { get; set; }
        public string TimeAgo { get; set; }
        public int? Status { get; set; }
    }
}
