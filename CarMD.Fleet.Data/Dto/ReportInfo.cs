using System;

namespace CarMD.Fleet.Data.Dto
{
    public class ReportInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreateDate { get; set; }
        public string YMM { get; set; }
        public string E { get; set; }
        public string YMME { get; set; }
        public string Vin { get; set; }
        public decimal Mileage { get; set; }
        public Guid ReportId { get; set; }
        public long ReportNumber { get; set; }
    }
}
