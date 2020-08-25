using CarMD.Fleet.Data.Response.Api;
using Innova.Utilities.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class KioskPing : BaseEntity
    {
        public KioskPing()
        {
        }
        public long KioskId { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public int Status { get; set; }
        public int? Count { get; set; }
        public bool? IsSendEmail { get; set; }

        public virtual Kiosk Kiosk { get; set; }
    }
}
