using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMD.Fleet.Data.Dto
{
    public class VehicleInfo
    {
        public string Vin { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Aaia { get; set; }
        public string EngineType { get; set; }
        public string Manufacture { get; set; }
        public bool IsActive { get; set; }
        public string TrimLevel { get; set; }
        public string Transmission { get; set; }
        public string Tag { get; set; }
        public string ModelImageFileUrl { get; set; }
        public int Mileage { get; set; }
        public string NickName { get; set; }
    }
}
