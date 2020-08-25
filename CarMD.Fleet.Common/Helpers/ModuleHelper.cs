using System;

namespace CarMD.Fleet.Common.Helper
{

    public static class ModuleHelper
    {
        public static string GetModuleName(string moduleName)
        {
            if (!string.IsNullOrWhiteSpace(moduleName))
            {
                if (moduleName.IndexOf(" - ") >= 0)
                {
                    var item = moduleName.Substring(moduleName.IndexOf(" - ") + 3).Trim();
                    return item;
                }
                else if (moduleName.LastIndexOf("-") >= 0)
                {
                    var item = moduleName.Substring(moduleName.IndexOf("-") + 1).Trim();
                    return item;
                }
                return moduleName;
            }
            return "N/A";
        }

        public static string GetModuleShortName(string moduleName)
        {
            if (!string.IsNullOrWhiteSpace(moduleName))
            {
                if (moduleName.IndexOf(" - ") >= 0)
                {
                    var item = moduleName.Substring(0, moduleName.IndexOf(" - ")).Trim();
                    return item;
                }
                else if (moduleName.IndexOf("-") >= 0)
                {
                    var item = moduleName.Substring(0, moduleName.IndexOf("-")).Trim();
                    return item;
                }
                return moduleName.Length > 3 ? moduleName.Substring(0, 3) : moduleName;
            }
            return "N/A";
        }
    }
}
