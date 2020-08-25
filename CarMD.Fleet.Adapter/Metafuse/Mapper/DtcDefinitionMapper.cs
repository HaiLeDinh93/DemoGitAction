using MetafuseReference;
using CarMD.Fleet.Core.Mapper;
using CarMD.Fleet.Data.Dto;
using System.Globalization;

namespace CarMD.Fleet.Adapter.Metafuse.Mapper
{
    internal sealed class DtcDefinitionMapper : BaseMapper<ErrorCodeInfoDefinition, DtcDefinition>
    {
        public override void Map(ErrorCodeInfoDefinition source, DtcDefinition target)
        {
            target.Conditions = source.Conditions;
            target.LaymansTermsSeverityLevel = source.LaymansTermSeverityLevel;
            target.LaymansTermsConditions = source.LaymansTermsConditions;
            target.LaymansTermsTitle = source.LaymansTermsTitle;
            target.LaymansTermDescription = source.LaymansTermDescription;
            target.LaymansTermsSeverityLevelDefinition = source.LaymansTermSeverityLevelDefinition;
            target.LaymansTermEffectOnVehicle = source.LaymansTermEffectOnVehicle;
            target.LaymansTermResponsibleComponentOrSystem = source.LaymansTermResponsibleComponentOrSystem;
            target.LaymansTermWhyItsImportant = source.LaymansTermWhyItsImportant;

            target.MessageIndicatorLampFile = source.MessageIndicatorLampFile;
            target.MessageIndicatorLampFileUrl = source.MessageIndicatorLampFileUrl;
            target.MonitorFile = source.MonitorFile;
            target.MonitorFileUrl = source.MonitorFileUrl;
            target.MonitorType = source.MonitorType;
            target.PassiveAntiTheftIndicatorLampFile = source.PassiveAntiTheftIndicatorLampFile;
            target.PassiveAntiTheftIndicatorLampFileUrl = source.PassiveAntiTheftIndicatorLampFileUrl;
            target.PossibleCauses = source.PossibleCauses;
            target.ServiceThrottleSoonIndicatorLampFile = source.ServiceThrottleSoonIndicatorLampFile;
            target.ServiceThrottleSoonIndicatorLampFileUrl = source.ServiceThrottleSoonIndicatorLampFileUrl;
            target.Title = source.Title;
            target.TransmissionControlIndicatorLampFile = source.TransmissionControlIndicatorLampFile;
            target.TransmissionControlIndicatorLampFileUrl = source.TransmissionControlIndicatorLampFileUrl;
            target.Trips = source.Trips.ToString(CultureInfo.InvariantCulture);

            if (source.ErrorCodeDefinitionVehicles == null || source.ErrorCodeDefinitionVehicles.Length <= 0) return;
            target.BodyCode = source.ErrorCodeDefinitionVehicles[0].BodyCode;
            target.EngineType = source.ErrorCodeDefinitionVehicles[0].EngineType;
        }
    }
}
