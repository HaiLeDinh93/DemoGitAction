using System;

namespace CarMD.Fleet.Data.Response.Api
{
    [Serializable]
    public class UserModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AuthorizationKey { get; set; }

        public string Email { get; set; }

        public string MobilePhone { get; set; }

        public string TimeZone { get; set; }

        public long KioskId { get; set; }
        
        public KioskModel Kiosk { get; set; }
    }
}
