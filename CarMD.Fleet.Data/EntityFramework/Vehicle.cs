using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class Vehicle :  BaseEntity
    {
        public Guid ConsumerId { get; set; }
        public string Vin { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Transmission { get; set; }
        public string TrimLevel { get; set; }
        public string Aaia { get; set; }
        public string EngineType { get; set; }
        public string Manufacture { get; set; }
        public int Mileage { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string ImageFileUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Consumer Consumer { get; set; }

        public Vehicle() { }

        public Vehicle(Report source)
        {
            Id = Guid.NewGuid();
            Vin = source.Vin;
            Year = source.Year;
            Make = source.Make;
            Model = source.Model;
            Transmission = source.Transmission;
            TrimLevel = source.TrimLevel;
            Aaia = source.Aaia;
            EngineType = source.EngineType;
            Manufacture = source.Manufacture;
            Mileage = source.Mileage;
            IsActive = true;
            CreatedDate = DateTime.UtcNow;
        }
    }
}
