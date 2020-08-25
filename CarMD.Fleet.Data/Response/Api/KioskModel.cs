using System;

namespace CarMD.Fleet.Data.Response.Api
{
    [Serializable]
    public class KioskModel
    {
        public string KioskId { get; set; }
        public string ScannerId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}
