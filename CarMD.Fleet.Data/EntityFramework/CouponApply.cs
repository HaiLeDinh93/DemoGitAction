using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class CouponApply
    {
        public Guid Id { get; set; }
        public Guid CouponId { get; set; }
        public string CouponCode { get; set; }
        public string ApplyTo { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public virtual Coupon IdNavigation { get; set; }
    }
}
