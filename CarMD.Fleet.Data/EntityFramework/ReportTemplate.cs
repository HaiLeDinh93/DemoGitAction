using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class ReportTemplate
    {
        public int Id { get; set; }
        public Guid? KioskId { get; set; }
        public string ReportHtml { get; set; }
        public int Type { get; set; }
        public bool? IncludeCouponPage { get; set; }
        public bool? ShowOnlyAvailableContent { get; set; }
        public string HiddenContent { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string BannerImage { get; set; }
    }
}
