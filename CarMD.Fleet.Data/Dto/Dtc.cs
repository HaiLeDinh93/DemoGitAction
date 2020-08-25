using CarMD.Fleet.Common.Enum;
using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.Dto
{
    public class Dtc
    {
        public Dtc()
        {
            Definitions = new List<DtcDefinition>();
        }

        public Guid Id { get; set; }
        public string Code { get; set; }
        public ErrorCodeLevel CodeLevel { get; set; }
        public ErrorCodeSystemType CodeType { get; set; }
        public IList<DtcDefinition> Definitions { get; set; }
    }
}
