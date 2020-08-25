using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMD.Fleet.Data.Dto
{
    public class DtcDefinition
    {
        public Guid Id { get; set; }
        public string Conditions { get; set; }
        public string BodyCode { get; set; }
        public string EngineType { get; set; }
        public int LaymansTermsSeverityLevel { get; set; }
        public string LaymansTermsConditions { get; set; }
        public string LaymansTermsTitle { get; set; }
        public string LaymansTermDescription { get; set; }
        public string LaymansTermsSeverityLevelDefinition { get; set; }
        public string LaymansTermEffectOnVehicle { get; set; }
        public string LaymansTermResponsibleComponentOrSystem { get; set; }
        public string LaymansTermWhyItsImportant { get; set; }

        public string MessageIndicatorLampFile { get; set; }
        public string MessageIndicatorLampFileUrl { get; set; }
        public string MonitorFile { get; set; }
        public string MonitorFileUrl { get; set; }
        public string MonitorType { get; set; }
        public string PassiveAntiTheftIndicatorLampFile { get; set; }
        public string PassiveAntiTheftIndicatorLampFileUrl { get; set; }
        public string PossibleCauses { get; set; }
        public string ServiceThrottleSoonIndicatorLampFile { get; set; }
        public string ServiceThrottleSoonIndicatorLampFileUrl { get; set; }
        public string Title { get; set; }
        public string TransmissionControlIndicatorLampFile { get; set; }
        public string TransmissionControlIndicatorLampFileUrl { get; set; }
        public string Trips { get; set; }
    }
}
