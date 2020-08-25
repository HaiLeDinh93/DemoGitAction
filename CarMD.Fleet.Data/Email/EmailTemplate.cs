using System.Collections.Generic;

namespace CarMD.Fleet.Data.Email
{
    public class EmailTemplate
    {
        public string Subject { get; set; }
        public List<string> Emails { get; set; }
    }
}
