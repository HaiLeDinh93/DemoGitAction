using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class FeedBack: BaseEntity
    {
        public long KioskId { get; set; }
        public Guid UserId { get; set; }
        public int Type { get; set; }
        public string CustomerEmail { get; set; }
        public Guid? ReportId { get; set; }
        public bool? LikeThis { get; set; }
        public string Comment { get; set; }
        public bool IsSent { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
