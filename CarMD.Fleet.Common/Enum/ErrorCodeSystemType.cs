using System.ComponentModel;

namespace CarMD.Fleet.Common.Enum
{
    public enum ErrorCodeSystemType
    {
        [Description("PowerTrain")]
        Powertrain = 0,

        [Description("Obd1")]
        Obd1 = 1,

        [Description("Abs")]
        Abs = 2,

        [Description("Srs")]
        Srs = 3,

        [Description("Enhanced")]
        Enhanced = 4
    }
}
