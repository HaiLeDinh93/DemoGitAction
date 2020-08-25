using CarMD.Fleet.Data.Request;
using System;

namespace CarMD.Fleet.Data.Dto.Metafuse
{
    [Serializable]
    public class TsbSearchCriteria : SearchCriteria
    {
        public string WebServiceKey { get; set; }
        public string Vin { get; set; }
        public string Category { get; set; }
        public int TSBCategoryId { get; set; }
        public string CommaSeparatedErrorCodes { get; set; }
    }
}
