using CarMD.Fleet.Data.Response.Api;
using Innova.Utilities.Shared.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class Kiosk : BaseEntity
    {
        public Kiosk()
        {
            Coupon = new HashSet<Coupon>();
            User = new HashSet<User>();
            KioskPing = new HashSet<KioskPing>();
        }

        public new long Id { get; set; }
        public string ScannerId { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Logo { get; set; }
        public string MechanicUrl { get; set; }
        public string EmailNotification { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<Coupon> Coupon { get; set; }
        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<KioskPing> KioskPing { get; set; }

        [NotMapped]
        public string KioskId
        {
            get { return Id.ToString().PadLeft(6, '0'); }
        }

        public KioskModel ToApiDto()
        {
            var target = new KioskModel
            {
                Address1 = Address,
                Address2 = Address1,
                City = City,
                Country = Country,
                PostalCode = PostalCode,
                ScannerId = ScannerId,
                State = State,
                KioskId = Id.ToString().PadLeft(6, '0')
            };
            return target;
        }
    }
}
