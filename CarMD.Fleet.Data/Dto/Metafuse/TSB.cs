using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.Dto.Metafuse
{
    public class TSB
    {
        public string AutoSystem { get; set; }
        public string TSBText { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public DateTime? IssueDate { get; set; }
        public string Description { get; set; }
        public string FileNamePDF { get; set; }
        public string ManufacturerNumber { get; set; }
        public int TSBID { get; set; }
        public string PDFFileUrl { get; set; }

        public List<string> DTCs { get; set; }
        public List<int> TsbCategories { get; set; }
        public List<string> TsbTypes { get; set; }
    }
}
