using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMD.Fleet.Common.Enum
{
    public enum AdminPermission
    {
        Full_Permission = 999,

        Account_View = 0,
        Account_Create = 1,
        Account_Edit = 2,
        Account_Delete = 3,

        Kiosk_View = 10,
        Kiosk_Create = 11,
        Kiosk_Edit = 12,
        Kiosk_Delete = 13,

    }
}