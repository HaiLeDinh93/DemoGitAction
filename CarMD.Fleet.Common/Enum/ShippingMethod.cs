using System.ComponentModel;

namespace CarMD.Fleet.Common.Enum
{
    public enum ShippingMethod
    {
        [Description("FedEx Express Delivery")]
        FedExExpress = 3,

        [Description("FedEx Home Delivery")]
        FedExHome = 90,

        [Description("SmartPost")]
        FedExSmartPost = 91
    }
}
