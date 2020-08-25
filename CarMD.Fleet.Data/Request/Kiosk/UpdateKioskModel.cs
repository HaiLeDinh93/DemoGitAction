using System;
using System.ComponentModel.DataAnnotations;

namespace CarMD.Fleet.Data.Request.Kiosk
{
    public class UpdateKioskModel
    {
        public UpdateKioskModel() { }
        public UpdateKioskModel(EntityFramework.Kiosk model)
        {
            Id = model.Id;
            Address1 = model.Address;
            Address2 = model.Address1;
            City = model.City;
            State = model.State;
            Country = model.Country;
            PostalCode = model.PostalCode;
        }
        public long Id { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string PostalCode { get; set; }

        public string UpdatedBy { get; set; }
    }
}
