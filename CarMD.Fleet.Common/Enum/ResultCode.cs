using System.ComponentModel;

namespace CarMD.Fleet.Common.Enum
{
    public enum ResultCode
    {
        [Description("Ok")]
        Ok = 0,

        [Description("Data Invaild")]
        DataInvaild = 1,

        [Description("Exception")]
        Error = 2,

        #region Web APIs Exx1xxx

        [Description("Authorization Key is required.")]
        E001001 = 1001,

        [Description("Invalid Authorization Key.")]
        E001002 = 1002,

        #endregion

        #region User Exx2xxx

        [Description("User Not Found.")]
        E002000 = 2000,

        [Description("User Name Existed.")]
        E002001 = 2001,

        [Description("Password Is Incorrect.")]
        E002002 = 2002,

        [Description("User is not actived.")]
        E002003 = 2003,

        [Description("Update User Error.")]
        E002004 = 2004,

        [Description("Invalid Email/Phone or Password.")]
        E002005 = 2005,
        #endregion

        #region DiagnosticReport Exx3xxx

        [Description("Diagnostic Report Not Found.")]
        E003000 = 3000,
        [Description("Create Diagnostic Report Error.")]
        E003001 = 3001,
        [Description("Update Diagnostic Report Error.")]
        E003002 = 3002,
        [Description("Report module not found.")]
        E003003 = 3003,
        [Description("Diagnostic Report result from Metafuse is null or has error.")]
        E003004 = 3004,

        #endregion

        #region Vehicle Exx5xxx
        [Description("Vehicle Not Found.")]
        E005000 = 5000,

        [Description("Create Vehicle Error.")]
        E005001 = 5001,

        [Description("Vin Existed.")]
        E005002 = 5002,

        [Description("Delete Vehicle Error.")]
        E005003 = 5003,

        [Description("Update Vehicle Error.")]
        E005004 = 5004,

        [Description("Vehicle Existed.")]
        E005005 = 5005,

        [Description("Cannot Decode Vin.")]
        E005006 = 5006,

        [Description("Cannot decode DLC Location.")]
        E005007 = 5007,

        #endregion
    }
}
