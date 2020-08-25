using CarMD.Fleet.Adapter.Metafuse.Mapper;
using MetafuseReference;
using CarMD.Fleet.Core.Mapper;
using CarMD.Fleet.Data.Dto;
using CarMD.Fleet.Data.Dto.Metafuse;
using CarMD.Fleet.Data.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using VehicleInfo = CarMD.Fleet.Data.Dto.VehicleInfo;

namespace CarMD.Fleet.Adapter.Metafuse
{
    public static class MetafuseMapper
    {
        private static readonly MapperRegister Register = new MapperRegister();

        
        static MetafuseMapper()
        {
            //register mapper here
            Register.Register(new DiagnosticReportMapper());

            Register.Register(new DtcMapper());
            Register.Register(new DtcDefinitionMapper());

            Register.Register(new FixMapper());
            Register.Register(new FixPartMapper());
            Register.Register(new FixPartOEMMapper());

            Register.Register(new ScheduleMaintenanceMapper());
            Register.Register(new RecallMapper());
            Register.Register(new WarrantyMapper());

            Register.Register(new PredictiveDiagnosticMapper());

            Register.Register(new VehicleMapper());

            Register.Register(new DLCLocationMapper());

            Register.Register(new MapTSBCategory());
            Register.Register(new TSBMapper());

            Register.Register(new FiveYearCostToOwnInfoMapper());
        }

        public static Data.Dto.Metafuse.DiagnosticReport MapDiagnosticReport(DiagReportInfo source)
        {
            var report = Register.Map<DiagReportInfo, Data.Dto.Metafuse.DiagnosticReport>(source);
            return report;
        }

        public static IList<Dtc> MapDtcs(IEnumerable<ErrorCodeInfo> errorCodeInfos)
        {
            return errorCodeInfos != null
                       ? Register.MapArray<ErrorCodeInfo, Dtc>(errorCodeInfos.ToArray())
                       : new List<Dtc>();
        }

        public static IList<DtcDefinition> MapDtcDefinition(ErrorCodeInfoDefinition[] source)
        {
            return source != null
                       ? Register.MapArray<ErrorCodeInfoDefinition, DtcDefinition>(source)
                       : new List<DtcDefinition>();
        }

        public static Fix MapFix(FixInfo fixInfo)
        {
            return fixInfo != null ? Register.Map<FixInfo, Fix>(fixInfo) : new Fix();
        }

        public static IList<Fix> MapFixes(FixInfo[] fixInfos)
        {
            return fixInfos != null ? Register.MapList<FixInfo, Fix>(fixInfos) : new List<Fix>();
        }

        public static FixPart MapFixPart(FixPartInfo fixPartInfo)
        {
            return Register.Map<FixPartInfo, FixPart>(fixPartInfo);
        }

        public static FixPartOEM MapFixPartOEM(FixPartOemInfo fixPartOemInfo)
        {
            return Register.Map<FixPartOemInfo, FixPartOEM>(fixPartOemInfo);
        }

        public static IList<Warranty> MapWarranties(VehicleWarrantyDetailInfo[] warrantyInfos)
        {
            return warrantyInfos != null ? Register.MapArray<VehicleWarrantyDetailInfo, Warranty>(warrantyInfos) : new List<Warranty>();
        }

        public static IList<Data.Dto.Metafuse.ScheduledMaintenance> MapScheduleMaintenances(ScheduleMaintenanceServiceInfo[] infos)
        {
            return infos != null ? Register.MapList<ScheduleMaintenanceServiceInfo, Data.Dto.Metafuse.ScheduledMaintenance>(infos) : new List<Data.Dto.Metafuse.ScheduledMaintenance>();
        }

        public static IList<Recall> MapRecalls(RecallInfo[] recallInfos)
        {
            return recallInfos != null ? Register.MapList<RecallInfo, Recall>(recallInfos) : new List<Recall>();
        }

        public static IList<PredictiveDiagnostic> MapPredictiveDiagnostic(PredictiveDiagnosticInfo predictiveDiagnosticInfo)
        {
            if (predictiveDiagnosticInfo == null)
            {
                return new List<PredictiveDiagnostic>();
            }

            var items = predictiveDiagnosticInfo.Fixes != null && predictiveDiagnosticInfo.Fixes.Any() ? predictiveDiagnosticInfo.Fixes : null;

            var results = items != null ? Register.MapList<FixInfo, PredictiveDiagnostic>(predictiveDiagnosticInfo.Fixes) : new List<PredictiveDiagnostic>();

            foreach (var item in results)
            {
                item.Engine = predictiveDiagnosticInfo.Engine;
                item.Make = predictiveDiagnosticInfo.Make;
                item.Year = predictiveDiagnosticInfo.Year;
                item.Model = predictiveDiagnosticInfo.Model;
                item.MileageRangeStart = predictiveDiagnosticInfo.MileageRangeStart;
                item.MileageRangeEnd = predictiveDiagnosticInfo.MileageRangeEnd;
                item.MileageRequested = predictiveDiagnosticInfo.MileageRequested;
            }

            return results;

        }

        public static VehicleInfo MapVehicle(MetafuseReference.VehicleInfo source)
        {
            return Register.Map<MetafuseReference.VehicleInfo, VehicleInfo >(source);
        }

        public static DLCLocation MapDLCLocation(DLCLocationInfo source)
        {
            return Register.Map<DLCLocationInfo, DLCLocation>(source);
        }

        public static IList<TSBCategory> MapTSBCategories(TSBCategoryInfo[] TSBCategories)
        {
            return TSBCategories != null ? Register.MapList<TSBCategoryInfo, TSBCategory>(TSBCategories) : new List<TSBCategory>();
        }

        public static IList<TSB> MapTSBs(TSBInfo[] TSBInfos)
        {
            return TSBInfos != null ? Register.MapList<TSBInfo, TSB>(TSBInfos) : new List<TSB>();
        }

        public static FiveYearCostToOwnInfo Map5YearCostToOwnInfo(VehicleCostToOwnInfo source)
        {
            var result = Register.Map<VehicleCostToOwnInfo, FiveYearCostToOwnInfo>(source);
            return result;
        }
    }
}
