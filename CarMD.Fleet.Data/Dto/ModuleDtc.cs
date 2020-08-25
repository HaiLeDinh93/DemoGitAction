using System.Runtime.Serialization;

namespace CarMD.Fleet.Data.Dto
{
    [DataContract]
    public class ModuleDtc
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Def { get; set; }

        [DataMember]
        public string Type { get; set; }
    }
}
