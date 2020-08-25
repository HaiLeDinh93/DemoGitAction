using System;


namespace CarMD.Fleet.Data.Request.Kiosk
{
    public class KioskSearchCriteria : SearchCriteria
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string EmailAddress { get; set; }
    }
}
