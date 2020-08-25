using System.ComponentModel;

namespace CarMD.Fleet.Common.Enum
{
    public enum EmissionsSmogTestStatus
    {
        [Description("NEEDS INSPECTION")]
        NeedsInspection,
        [Description("PASS")]
        Pass,
        [Description("WILL FAIL")]
        WillFail
    }
}
