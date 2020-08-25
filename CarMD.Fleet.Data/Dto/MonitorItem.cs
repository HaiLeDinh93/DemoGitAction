using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Core.Utility;
using Innova.Utilities.Shared.Tool;
using System;

namespace CarMD.Fleet.Data.Dto
{
    public class MonitorItem
    {
        public MonitorItem(MonitorInfo info)
        {
            Name = info.Description;
            Status = info.Value;
        }

        public string Name { get; set; }

        public string Status { get; set; }

        public bool IsNotSupported()
        {
            var notSupportedDescriptionText = EnumUtility.GetDescriptionOfEnumItem(typeof(MonitorStatus), (int)MonitorStatus.NotSupported);
            var notSupportedText = MonitorStatus.NotSupported.ToString();

            return !string.IsNullOrEmpty(Status) && (Status.Equals(notSupportedText, StringComparison.OrdinalIgnoreCase) || Status.Equals(notSupportedDescriptionText, StringComparison.OrdinalIgnoreCase));

        }

        public bool IsNotComplete()
        {
            var notCompleteDescriptionText = EnumUtility.GetDescriptionOfEnumItem(typeof(MonitorStatus), (int)MonitorStatus.NotComplete);
            var notCompleteText = MonitorStatus.NotComplete.ToString();

            return !string.IsNullOrEmpty(Status) && (Status.Equals(notCompleteText, StringComparison.OrdinalIgnoreCase) || Status.Equals(notCompleteDescriptionText, StringComparison.OrdinalIgnoreCase));

        }

        public string GetShortName()
        {
            switch (Name)
            {
                case "MIL": return "MIL";

                case "Misfire Monitoring":
                case "Misfire Monitor": return "M";

                case "Fuel System Monitoring":
                case "Fuel System Monitor": return "F";

                case "Comprehensive Component Monitoring":
                case "CCM Monitoring":
                case "CCM Monitor": return "CC";

                case "Catalyst Monitoring":
                case "Catalyst Monitor": return "C";

                case "Heated Catalyst Monitoring":
                case "Heated Catalyst Monitor": return "HC";

                case "Evaporative System Monitoring":
                case "EVAP Monitoring":
                case "EVAP Monitor": return "EV";

                case "Secondary Air System Monitoring":
                case "Secondary Air System Monitor": return "AC";

                case "Oxygen Sensor Monitoring":
                case "O2 Sensor Monitoring":
                case "O2 Sensor Monitor": return "O";


                case "Oxygen Sensor Heater Monitoring":
                case "O2 Sensor Heater Monitoring":
                case "O2 Sensor Heater Monitor": return "OH";

                case "EGR and/or VVT System Monitoring":
                case "EGR Monitoring":
                case "EGR Monitor": return "E";

                case "NMHC Catalyst Monitoring":
                case "NMHC Monitoring":
                case "NMHC Monitor": return "NM";

                case "NOx Aftertreatment Monitoring":
                case "Nox Adsorber Monitoring":
                case "Nox Adsorber Monitor": return "N";

                case "Boost Pressure System Monitoring":
                case "Boost Pressure System Monitor": return "BP";

                case "Exhaust Gas Sensor Monitoring":
                case "Exhaust Gas Sensor Monitor": return "EG";

                case "PM Filter Monitoring":
                case "PM Filter Monitor": return "P";

            }
            return string.IsNullOrEmpty(Name) ? "" : Name.Substring(0, 1);
        }
    }
}
