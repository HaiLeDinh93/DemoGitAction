using System;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class Admin : BaseEntity
    {
        public string UserName { get; set; }
        public string HashPassword { get; set; }
        public string TempPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string TimeZone { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastActiveDate { get; set; }
        public DateTime? LastDeactiveDate { get; set; }
        public string Permission { get; set; }
    }
}
