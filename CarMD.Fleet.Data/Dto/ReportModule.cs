using CarMD.Fleet.Common.Helper;
using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.Dto
{
    public class ReportModule
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }

        public string Name
        {
            get
            {
                return ModuleHelper.GetModuleName(FullName);
            }
        }

        public string ShortName
        {
            get
            {
                return ModuleHelper.GetModuleShortName(FullName);
            }
        }
        public string System { get; set; }
        public string SubSystem { get; set; }
        public int DTCCount { get; set; }
        public List<ReportModuleDTC> dtcs { get; set; }
    }
}
