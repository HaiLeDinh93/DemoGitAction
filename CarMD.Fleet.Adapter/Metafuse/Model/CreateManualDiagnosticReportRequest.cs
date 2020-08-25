using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMD.Fleet.Adapter.Metafuse.Model
{
    public class CreateManualDiagnosticReportRequest : CreateReportRequest
    {
        public string PwrPrimaryDtc { get; set; }
        public string PwrStoredCodesCommaSeparatedList { get; set; }
        public string PwrPendingCodesCommaSeparatedList { get; set; }
        public string PwrPermanentCodesCommaSeparatedList { get; set; }
        public string Obd1StoredCodesCommaSeparatedList { get; set; }
        public string Obd1PendingCodesCommaSeparatedList { get; set; }
        public string AbsStoredCodesCommaSeparatedList { get; set; }
        public string AbsPendingCodesCommaSeparatedList { get; set; }
        public string SrsStoredCodesCommaSeparatedList { get; set; }
        public string SrsPendingCodesCommaSeparatedList { get; set; }
        public new string UserId { get; set; }
    }
}
