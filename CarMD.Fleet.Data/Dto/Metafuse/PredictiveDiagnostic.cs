
namespace CarMD.Fleet.Data.Dto.Metafuse
{
    public class PredictiveDiagnostic : Fix
    {
        public string Engine { get; set; }

        public string Make { get; set; }

        public int Year { get; set; }

        public string Model { get; set; }

        public int MileageRangeEnd { get; set; }

        public int MileageRangeStart { get; set; }

        public int MileageRequested { get; set; }

        public int Status { get; set; }
    }
}
