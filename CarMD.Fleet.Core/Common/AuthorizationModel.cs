using System;

namespace CarMD.Fleet.Core.Common
{
    [Serializable]
    public class AuthorizationModel
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
