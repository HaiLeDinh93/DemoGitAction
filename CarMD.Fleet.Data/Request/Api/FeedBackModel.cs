using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.Request.Api
{
    public class FeedBackModel
    {
        public long KioskId { get; set; }

        public Guid UserId { get; set; }

        public string UserEmail { get; set; }

        public List<string> ToEmails { get; set; }

        public int Type { get; set; }

        public string CustomerEmail { get; set; }

        public string Comment { get; set; }

        public bool? LikeThis { get; set; }

        public Guid? ReportId { get; set; }
    }
}
