using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMD.Fleet.Adapter.Metafuse.Model
{
    public class VehicleBaseRequest
    {
        public string ServiceKey { get; set; }
        public string Vin { get; set; }
        public string Class { get; set; }
        public string Manufacturer { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public int? Year { get; set; }
        public string EngineType { get; set; }
        public string EngineVinCode { get; set; }
        public string Transmission { get; set; }
        public string AAIA { get; set; }
        public int? CurrentMileage { get; set; }
    }
}
