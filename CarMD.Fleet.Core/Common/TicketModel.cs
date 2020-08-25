using System;

namespace CarMD.Fleet.Core.Common
{
    [Serializable]
    public class TicketModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime DateTime { get; set; }

        public int interval { get; set; }
    }
}
