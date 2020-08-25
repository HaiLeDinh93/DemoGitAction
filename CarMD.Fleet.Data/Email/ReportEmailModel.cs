using System;


namespace CarMD.Fleet.Data.Email
{
    public class ReportEmailModel : EmailTemplate
    {
        public string YMM { get; set; }

        public string Mileage { get; set; }

        public int CheckEngineLightStatus { get; set; }

        public int TotalErrorCodeCount { get; set; }

        public int RepairsCount { get; set; }

        public string ReportUrl { get; set; }

        public string ImagePath { get; set; }

        public bool HasMilFix { get; set; }
    }
}
