namespace CarMD.Fleet.Data.Dto
{
    public class DLCLocation
    {
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string EngineType { get; set; }
        public string Vin { get; set; }
        public int? LocationNumber { get; set; }
        public string Access { get; set; }
        public string Comments { get; set; }
        public string ImageFileName { get; set; }
        public string ImageFileUrl { get; set; }
        public string ImageFileUrlSmall { get; set; }

        public ReportHistory Report { get; set; }
    }
}
