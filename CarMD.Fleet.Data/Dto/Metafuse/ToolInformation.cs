using CarMD.Fleet.Data.EntityFramework;
using Innova.Utilities.Shared.Tool;
using System.Collections.Generic;


namespace CarMD.Fleet.Data.Dto.Metafuse
{
    public class ToolInformation
    {
        public ToolInformation()
        {
            Monitors = new List<MonitorInfo>();

            FreezeFrames = new List<FreezeFrameInfo>();
        }

        public string FirmwareVersion { get; set; }

        public int? SoftwareType { get; set; }

        public string SoftwareVersion { get; set; }

        public int? ToolTypeFormat { get; set; }

        public string ToolId { get; set; }

        public string Vin { get; set; }

        public string PrimaryDtc { get; set; }

        public List<string> AllErrorCodes { get; set; }

        public List<string> StoredOBD1Codes { get; set; }

        public List<string> PendingOBD1Codes { get; set; }

        public List<string> StoredAbsCodes { get; set; }

        public List<string> PendingAbsCodes { get; set; }

        public List<string> StoredSrsCodes { get; set; }

        public List<string> PendingSrsCodes { get; set; }

        public List<string> StoredPowerTrainsCodes { get; set; }

        public List<string> PendingPowerTrainsCodes { get; set; }

        public List<string> PermanentPowerTrainsCodes { get; set; }

        public List<string> EnhancedCodes { get; set; }

        public List<MonitorInfo> Monitors { get; set; }

        public List<FreezeFrameInfo> FreezeFrames { get; set; }

        public int? ToolLEDStatus { get; set; }

        public int? ToolMilStatus { get; set; }

    }
}
