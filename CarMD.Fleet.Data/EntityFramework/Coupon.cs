using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class Coupon
    {
        public Guid Id { get; set; }
        public long? KioskId { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string AdditionalFinePrint { get; set; }
        public decimal Value { get; set; }
        public int Type { get; set; }
        public int Target { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool IsTemplate { get; set; }
        public bool IsSuppended { get; set; }
        public string MaintenanceMapperIds { get; set; }
        public string Logo { get; set; }
        public int? Used { get; set; }

        public virtual Kiosk Kiosk { get; set; }
        public virtual CouponApply CouponApply { get; set; }
    }
}
