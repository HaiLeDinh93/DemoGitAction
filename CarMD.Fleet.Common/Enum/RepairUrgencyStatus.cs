using System.ComponentModel;

namespace CarMD.Fleet.Common.Enum
{
    public enum RepairUrgencyStatus
    {
        [Description("Low")]
        Low = 1,
        [Description("Med")]
        Medium = 2,
        [Description("High")]
        High = 3,
    }
}
