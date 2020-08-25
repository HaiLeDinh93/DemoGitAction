using CarMD.Fleet.Common.Helper;
using CarMD.Fleet.Core.Common;
using CarMD.Fleet.Core.Cryptography;

using System;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class User : BaseEntity
    {
        public long KioskId { get; set; }
        public string HashPassword { get; set; }
        public string TempPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string TimeZone { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastActiveDate { get; set; }
        public DateTime? LastDeactiveDate { get; set; }
        public bool Auth0Login { get; set; }
        public virtual Kiosk Kiosk { get; set; }


        public User()
        {
        }
        
        public Data.Response.Api.UserModel ToApiDto()
        {
            var target = new Data.Response.Api.UserModel();

            target.Email = Email;
            target.FirstName = FirstName;
            target.Id = Id;
            target.LastName = LastName;
            target.MobilePhone = MobilePhone;
            target.TimeZone = TimeZone;
            target.AuthorizationKey = AuthorizationHelper.CreateAuthorizationKey(new AuthorizationModel { Email = Email, Password = HashPassword });

            if (Kiosk != null)
            {
                target.KioskId = Kiosk.Id;
                target.Kiosk = Kiosk.ToApiDto();
            }

            return target;
        }
    }
}
