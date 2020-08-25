using System.ComponentModel;

namespace CarMD.Fleet.Common.Enum
{
    public enum ErrorCodeLevel
    {
        [Description("Primary Code")]
        PrimaryErrorCode = 0,

        [Description("First Stored Code")]
        FirstStoredCode = 1,

        [Description("Additional Stored Code")]
        AdditionalStoredCode = 2,

        [Description("First Pending Code")]
        FirstPendingCode = 3,

        [Description("Additional Pending Code")]
        AdditionalPendingCode = 4,

        [Description("First Permanent Code")]
        FirstPermanentCode = 5,

        [Description("Additional Permanent Code")]
        AdditionalPermanentCode = 6
    }
}
