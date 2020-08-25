using System;
using System.ComponentModel;

namespace CarMD.Fleet.Common.Enum
{
    [Serializable]
    public enum MonitorStatus
    {
        [Description("ON")]
        On,
        [Description("OFF")]
        Off,
        [Description("Not Supported")]
        NotSupported,
        [Description("Complete")]
        Complete,
        [Description("Not complete")]
        NotComplete
    }

    [Serializable]
    public enum MonitorEsStatus
    {
        [Description("ENCENDIDO")]
        MilOn,
        [Description("APAGADO")]
        MilOff,
        [Description("No soportado")]
        NotSupported,
        [Description("Completo")]
        Complete,
        [Description("Incompleto")]
        NotComplete
    }
}
