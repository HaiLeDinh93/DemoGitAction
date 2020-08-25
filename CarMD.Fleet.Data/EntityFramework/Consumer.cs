using CarMD.Fleet.Core.Cryptography;

using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class Consumer : BaseEntity
    {
        public Consumer()
        {
            Vehicle = new HashSet<Vehicle>();
        }

        public long KioskId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string TimeZone { get; set; }
        public bool IsActive { get; set; }
        public string HashPassword { get; set; }
        public string TempPassword { get; set; }
        public string Logo { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public bool Auth0Login { get; set; }

        public virtual ICollection<Vehicle> Vehicle { get; set; }
    }
}
