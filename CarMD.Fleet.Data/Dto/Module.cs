using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CarMD.Fleet.Data.Dto
{
    [DataContract]
    public class Module
    {
        [DataMember]
        public string ModuleName { get; set; }

        [DataMember]
        public string SubSystem { get; set; }

        [DataMember]
        public string System { get; set; }

        [DataMember]
        public int? InnovaGroup { get; set; }

        [DataMember]
        public List<ModuleDtc> Dtcs { get; set; }
    }
}
