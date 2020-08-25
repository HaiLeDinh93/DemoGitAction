using System.Runtime.Serialization;

namespace CarMD.Shell.Api.Models
{
    [DataContract]
    public class MissingDongle
    {
        [DataMember]
        public string CustomerEmail { get; set; }
    }
}