using CarMD.Fleet.Core.Utility;
using Innova.Utilities.Shared.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using CarMD.Fleet.Core.Utility.Extensions;

namespace CarMD.Fleet.Data.Dto.Metafuse
{
    public class DiagnosticReport
    {
        #region Constants

        const string MONITORS = "Monitors";
        const string FREEZE_FRAMES = "FreezeFrames";

        #endregion

        public DiagnosticReport()
        {
            Errors = new List<Dtc>();
            Fixes = new List<Fix>();

            SecondaryErrorCodes = new List<string>();
            StoredErrorCodes = new List<string>();
            PendingErrorCodes = new List<string>();
            ErrorCodes = new List<string>();
            ABSCurrentCodes = new List<string>();
            ABSHistoryCodes = new List<string>();
            SRSCurrentCodes = new List<string>();
            SRSHistoryCodes = new List<string>();
            OBD1CurrentCodes = new List<string>();
            OBD1HistoryCodes = new List<string>();

            Monitors = new List<MonitorInfo>();
            FreezeFrames = new List<FreezeFrameInfo>();
            Recalls = new List<Recall>();
        }

        public Guid DiagnosticReportId { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public string Aaia { get; set; }
        public string EngineType { get; set; }
        public bool IsActive { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string TrimLevel { get; set; }
        public string Transmission { get; set; }
        public string Manufacture { get; set; }
        public string Vin { get; set; }
        public int Mileage { get; set; }
        public string RawUploadString { get; set; }

        public IList<Recall> Recalls { get; set; }
        public string RecallItems { get; set; }
        public IList<Warranty> Warranties { get; set; }

        public int ScheduledMaintenanceNextMileage { get; set; }
        public IList<ScheduledMaintenance> ScheduledMaintenances { get; set; }

        public int UnScheduledMaintenanceNextMileage { get; set; }
        public IList<ScheduledMaintenance> UnScheduledMaintenanceServices { get; set; }

        public IList<PredictiveDiagnostic> PredictiveDiagnostics { get; set; }

        public List<MonitorInfo> Monitors { get; set; }
        public List<FreezeFrameInfo> FreezeFrames { get; set; }

        public int? ToolMilStatus { get; set; }
        public int? ToolLEDStatus { get; set; }
        public int? RecallCount { get; set; }
        public int? TsbCount { get; set; }

        public IList<Dtc> Errors;
        public IList<Fix> Fixes;
        public string PrimaryErrorCode { get; set; }
        public string ErrorCodeList { get; set; }

        public string SecondaryErrorCodeList { get; set; }
        public string StoredErrorCodeList { get; set; }
        public string PendingErrorCodeList { get; set; }

        public string PermanentCodeList { get; set; }
        public string ABSCurrentCodeList { get; set; }
        public string ABSHistoryCodeList { get; set; }
        public string SRSCurrentCodeList { get; set; }
        public string SRSHistoryCodeList { get; set; }
        public string OBD1CurrentCodeList { get; set; }
        public string OBD1HistoryCodeList { get; set; }
        public string Monitor { get; set; }
        public string FreezeFrame { get; set; }

        public List<string> ErrorCodes;
        public List<string> PendingErrorCodes;
        public List<string> StoredErrorCodes;
        public List<string> SecondaryErrorCodes;
        public List<string> PermanentCodes;
        public List<string> ABSHistoryCodes;
        public List<string> ABSCurrentCodes;
        public List<string> SRSHistoryCodes;
        public List<string> SRSCurrentCodes;
        public List<string> OBD1CurrentCodes;
        public List<string> OBD1HistoryCodes;

        #region Mapping

        public void Map()
        {
            ErrorCodeList = CollectionUtility.ToString(ErrorCodes);
            PendingErrorCodeList = CollectionUtility.ToString(PendingErrorCodes);
            PermanentCodeList = CollectionUtility.ToString(PermanentCodes);
            StoredErrorCodeList = CollectionUtility.ToString(StoredErrorCodes);
            SecondaryErrorCodeList = CollectionUtility.ToString(SecondaryErrorCodes);
            ABSHistoryCodeList = CollectionUtility.ToString(ABSHistoryCodes);
            ABSCurrentCodeList = CollectionUtility.ToString(ABSCurrentCodes);
            SRSHistoryCodeList = CollectionUtility.ToString(SRSHistoryCodes);
            SRSCurrentCodeList = CollectionUtility.ToString(SRSCurrentCodes);
            OBD1HistoryCodeList = CollectionUtility.ToString(OBD1HistoryCodes);
            OBD1CurrentCodeList = CollectionUtility.ToString(OBD1CurrentCodes);
            Monitor = ToolUtility.Convert2XmlString(MONITORS, Monitors);
            FreezeFrame = ToolUtility.Convert2XmlString(FREEZE_FRAMES, FreezeFrames);

        }

        public void ReMap()
        {
            ErrorCodes = ErrorCodeList.ToList();
            PendingErrorCodes = PendingErrorCodeList.ToList();
            PermanentCodes = PermanentCodeList.ToList();
            StoredErrorCodes = StoredErrorCodeList.ToList();
            SecondaryErrorCodes = SecondaryErrorCodeList.ToList();
            ABSHistoryCodes = ABSHistoryCodeList.ToList();
            ABSCurrentCodes = ABSCurrentCodeList.ToList();
            SRSHistoryCodes = SRSHistoryCodeList.ToList();
            SRSCurrentCodes = SRSCurrentCodeList.ToList();
            OBD1HistoryCodes = OBD1HistoryCodeList.ToList();
            OBD1CurrentCodes = OBD1CurrentCodeList.ToList();

            Monitors = ToolUtility.Convert2Object<MonitorInfo>(MONITORS, Monitor) ?? new List<MonitorInfo>();
            FreezeFrames = ToolUtility.Convert2Object<FreezeFrameInfo>(FREEZE_FRAMES, FreezeFrame) ?? new List<FreezeFrameInfo>();
        }

        #endregion
    }
}
