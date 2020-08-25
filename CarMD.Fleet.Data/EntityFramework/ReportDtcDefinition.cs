using CarMD.Fleet.Data.Dto;
using System;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class ReportDtcDefinition
    {
        public Guid Id { get; set; }
        public Guid ReportDtcId { get; set; }
        public string Title { get; set; }
        public string Conditions { get; set; }
        public string PossibleCauses { get; set; }
        public string Trips { get; set; }
        public string MessageIndicatorLampFile { get; set; }
        public string MessageIndicatorLampFileUrl { get; set; }
        public int LaymansTermSeverityLevel { get; set; }
        public string LaymansTermsConditions { get; set; }
        public string LaymansTermsTitle { get; set; }
        public string LaymansTermDescription { get; set; }
        public string LaymansTermSeverityLevelDefinition { get; set; }
        public string LaymansTermEffectOnVehicle { get; set; }
        public string LaymansTermResponsibleComponentOrSystem { get; set; }
        public string LaymansTermWhyItsImportant { get; set; }
        public string MonitorFile { get; set; }
        public string MonitorType { get; set; }
        public string MonitorFileUrl { get; set; }
        public string PassiveAntiTheftIndicatorLampFile { get; set; }
        public string PassiveAntiTheftIndicatorLampFileUrl { get; set; }
        public string ServiceThrottleSoonIndicatorLampFile { get; set; }
        public string ServiceThrottleSoonIndicatorLampFileUrl { get; set; }
        public string TransmissionControlIndicatorLampFile { get; set; }
        public string TransmissionControlIndicatorLampFileUrl { get; set; }
        public string BodyCode { get; set; }
        public string EngineType { get; set; }

        public virtual ReportDtc ReportDtc { get; set; }

        public ReportDtcDefinition() { }
        public ReportDtcDefinition(DtcDefinition source)
        {
            Id = Guid.NewGuid();
            Title = source.Title;
            Conditions = source.Conditions;
            PossibleCauses = source.PossibleCauses;
            Trips = source.Trips;
            MessageIndicatorLampFile = source.MessageIndicatorLampFile;
            MessageIndicatorLampFileUrl = source.MessageIndicatorLampFileUrl;

            LaymansTermSeverityLevel = source.LaymansTermsSeverityLevel;
            LaymansTermsConditions = source.LaymansTermsConditions;
            LaymansTermsTitle = source.LaymansTermsTitle;
            LaymansTermDescription = source.LaymansTermDescription;
            LaymansTermSeverityLevelDefinition = source.LaymansTermsSeverityLevelDefinition;
            LaymansTermEffectOnVehicle = source.LaymansTermEffectOnVehicle;
            LaymansTermResponsibleComponentOrSystem = source.LaymansTermResponsibleComponentOrSystem;
            LaymansTermWhyItsImportant = source.LaymansTermWhyItsImportant;
            MonitorFile = source.MonitorFile;
            MonitorFileUrl = source.MonitorFileUrl;
            MonitorType = source.MonitorType;
            PassiveAntiTheftIndicatorLampFile = source.PassiveAntiTheftIndicatorLampFile;
            PassiveAntiTheftIndicatorLampFileUrl = source.PassiveAntiTheftIndicatorLampFileUrl;
            ServiceThrottleSoonIndicatorLampFile = source.ServiceThrottleSoonIndicatorLampFile;
            ServiceThrottleSoonIndicatorLampFileUrl = source.ServiceThrottleSoonIndicatorLampFileUrl;
            TransmissionControlIndicatorLampFile = source.TransmissionControlIndicatorLampFile;
            TransmissionControlIndicatorLampFileUrl = source.TransmissionControlIndicatorLampFileUrl;

            BodyCode = source.BodyCode;
            EngineType = source.EngineType;
        }
    }
}
