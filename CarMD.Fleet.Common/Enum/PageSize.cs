using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CarMD.Fleet.Common.Enum
{
    public enum PageSize
    {
        [Description("25")]
        Size1 = 25,
        [Description("50")]
        Size2 = 50,
        [Description("100")]
        Size3 = 100,
        [Description("250")]
        Size4 = 250,
        [Description("500")]
        Size5 = 500

    }
}
