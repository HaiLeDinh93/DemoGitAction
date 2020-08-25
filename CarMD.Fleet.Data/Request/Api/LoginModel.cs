using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CarMD.Fleet.Data.Request.Api
{
    [DataContract]
    public class LoginModel
    {
        [Required]
        [DataMember]
        public string U { get; set; }

        [Required]
        [DataMember]
        public string P { get; set; }
    }
}
