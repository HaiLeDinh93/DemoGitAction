using CarMD.Fleet.Common.Configuration;
using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Common.Helpers;
using CarMD.Fleet.Core.Utility.Extensions;
using CarMD.Fleet.Data.Dto;
using CarMD.Fleet.Data.Dto.Metafuse;
using CarMD.Fleet.Data.Email;
using CarMD.Fleet.Data.Response.Api;
using Innova.Utilities.Shared.Tool;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CarMD.Fleet.Data.EntityFramework
{
    public partial class Report : BaseEntity
    {
        public Report()
        {
            ReportDtc = new HashSet<ReportDtc>();
            ReportFix = new HashSet<ReportFix>();
            ReportModule = new HashSet<ReportModule>();
            ReportPredictiveDiagnostic = new HashSet<ReportPredictiveDiagnostic>();
            ReportRecall = new HashSet<ReportRecall>();
            ReportScheduledMaintenance = new HashSet<ReportScheduledMaintenance>();
            ReportUnScheduledMaintenance = new HashSet<ReportUnScheduledMaintenance>();
            ReportWarranty = new HashSet<ReportWarranty>();
        }

        public long KioskId { get; set; }
        public Guid UserId { get; set; }
        public Guid? ConsumerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BannerImage { get; set; }
        public long ReportNumber { get; set; }
        public Guid DiagnosticReportId { get; set; }
        public string WorkOrderNumber { get; set; }
        public string Imei { get; set; }
        public string Vin { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Transmission { get; set; }
        public string TrimLevel { get; set; }
        public string Aaia { get; set; }
        public string EngineType { get; set; }
        public string Manufacture { get; set; }
        public int Mileage { get; set; }
        public bool IsActive { get; set; }
        public decimal? OriginalTirePressure { get; set; }
        public decimal? Lftire { get; set; }
        public decimal? Rftire { get; set; }
        public decimal? Lrtire { get; set; }
        public decimal? Rrtire { get; set; }
        public string TirePressureUnit { get; set; }
        public decimal? OriginalBatteryVoltage { get; set; }
        public decimal? CurrentBatteryVoltage { get; set; }
        public int? OilLevel { get; set; }
        public int? BrakePadLife { get; set; }
        public int ToolMilStatus { get; set; }
        public int ToolLedstatus { get; set; }
        public string PrimaryErrorCode { get; set; }
        public string ErrorCodes { get; set; }
        public string SecondaryErrorCodes { get; set; }
        public string StoredErrorCodes { get; set; }
        public string PendingErrorCodes { get; set; }
        public string PermanentCodes { get; set; }
        public string SrsCurrentCodes { get; set; }
        public string SrsHistoryCodes { get; set; }
        public string Obd1CurrentCodes { get; set; }
        public string Obd1HistoryCodes { get; set; }
        public string AbsCurrentCodes { get; set; }
        public string AbsHistoryCodes { get; set; }
        public string Monitor { get; set; }
        public string MonitorFile { get; set; }
        public string FreezeFrame { get; set; }
        public int ScheduledMaintenanceNextMileage { get; set; }
        public int RecallCount { get; set; }
        public string RecallItems { get; set; }
        public int TsbCount { get; set; }
        public string MonitorStatusRaw { get; set; }
        public string FreezeFrameRaw { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public bool? IsVinMismatch { get; set; }
        public string ToolFirmware { get; set; }
        public int? Status { get; set; }
        public int? UnScheduledMaintenanceNextMileage { get; set; }
        public string EcmdtcsRaw { get; set; }
        public string TcmdtcsRaw { get; set; }
        public int? BatteryStatus { get; set; }
        public bool? Opportunity { get; set; }
        public DateTime? OpportunityDate { get; set; }
        public string RawString { get; set; }
        public decimal? DepreciationCost { get; set; }
        public decimal? FuelCost { get; set; }
        public decimal? RepairCost { get; set; }
        public decimal? InsuranceCost { get; set; }
        public decimal? MaintenanceCost { get; set; }
        public decimal? TotalCostToOwn { get; set; }
        public string VehicleImageFileUrl { get; set; }

        public virtual ICollection<ReportDtc> ReportDtc { get; set; }
        public virtual ICollection<ReportFix> ReportFix { get; set; }
        public virtual ICollection<ReportModule> ReportModule { get; set; }
        public virtual ICollection<ReportPredictiveDiagnostic> ReportPredictiveDiagnostic { get; set; }
        public virtual ICollection<ReportRecall> ReportRecall { get; set; }
        public virtual ICollection<ReportScheduledMaintenance> ReportScheduledMaintenance { get; set; }
        public virtual ICollection<ReportUnScheduledMaintenance> ReportUnScheduledMaintenance { get; set; }
        public virtual ICollection<ReportWarranty> ReportWarranty { get; set; }

        [NotMapped]
        public List<MonitorInfo> Monitors { get; set; }
        [NotMapped]
        public List<FreezeFrameInfo> FreezeFrames { get; set; }

        [NotMapped]
        const string MONITORS = "Monitors";

        [NotMapped]
        const string FREEZE_FRAMES = "FreezeFrames";


        public ReportEmailModel ToEmailModel(string reportUrl)
        {
            var target = new ReportEmailModel()
            {
                Subject = "Vehicle Health Report",
                ReportUrl = reportUrl,
                ImagePath = UrlHelper.Merge(CommonConfiguration.ShellConsumerRootUrl, CommonConfiguration.EmailTemplateImgPath),
                Emails = new List<string> { Email },
                YMM = string.Format("{0} {1} {2}", Year, Make, Model),
                Mileage = Mileage.ToString("N0"),
            };

            var dtcPrimary = ReportDtc != null ? ReportDtc.FirstOrDefault(v => v.Code.Equals(PrimaryErrorCode, StringComparison.OrdinalIgnoreCase) && v.CodeLevel == (int)ErrorCodeLevel.PrimaryErrorCode) : null;

            var fix = 0;
            if (dtcPrimary != null && ReportFix != null)
            {
                fix = ReportFix.Where(v => v.ReportDtcId == dtcPrimary.Id).DistinctBy(v => v.Name).OrderBy(v => v.SortOrder).Count();
            }


            target.TotalErrorCodeCount = dtcPrimary != null ? 1 : 0;


            var ListABS_SRS = new List<ReportModule>();

            if (ReportModule != null)
            {
                //Shell Kiosk Report – Bugs and Issues (s8.1) 
                foreach (var item in ReportModule)
                {
                    item.ReportModuleDtc = item.GetReportModuleDtcs(PrimaryErrorCode);
                }

                var modulesHasIssues = ReportModule.Where(m => m.ReportModuleDtc != null && m.ReportModuleDtc.Any()).ToList();

                foreach (var item in modulesHasIssues)
                {
                    if (item.InnovaGroup.HasValue && !string.IsNullOrEmpty(item.ModuleName) &&
                       (item.InnovaGroup.Value == (int)InnovaGroup.ABS || item.InnovaGroup.Value == (int)InnovaGroup.SRS
                       || item.ModuleName.StartsWith("ABS", StringComparison.OrdinalIgnoreCase) || item.ModuleName.StartsWith("SRS", StringComparison.OrdinalIgnoreCase)))
                    {
                        ListABS_SRS.Add(item);
                    }
                }

                target.TotalErrorCodeCount += modulesHasIssues.Sum(v => v.ReportModuleDtc.Count());
            }

            var abs_srs = ListABS_SRS.Sum(v => v.ReportModuleDtc.Count());

            target.RepairsCount = (ReportScheduledMaintenance != null ? ReportScheduledMaintenance.Count : 0)
                     + (ReportPredictiveDiagnostic != null ? ReportPredictiveDiagnostic.Count : 0)
                     + fix
                     + abs_srs;
          
            target.CheckEngineLightStatus = !string.IsNullOrEmpty(PrimaryErrorCode) ? (int)MeterStatus.Red : (target.TotalErrorCodeCount > 0 ? (int)MeterStatus.Yellow : (int)MeterStatus.Green);

            target.HasMilFix = !string.IsNullOrEmpty(PrimaryErrorCode) && ReportFix != null && ReportFix.Any(v => v.ErrorCode.Equals(PrimaryErrorCode, StringComparison.OrdinalIgnoreCase));

            return target;
        }

        public Report(DiagnosticReport diagnosticReport)
        {
            Id = Guid.NewGuid();
            DiagnosticReportId = diagnosticReport.DiagnosticReportId;

            Year = diagnosticReport.Year;
            Make = diagnosticReport.Make;
            Model = diagnosticReport.Model;
            EngineType = diagnosticReport.EngineType;
            Manufacture = diagnosticReport.Manufacture;
            Aaia = diagnosticReport.Aaia;
            IsActive = diagnosticReport.IsActive;
            Transmission = diagnosticReport.Transmission;
            TrimLevel = diagnosticReport.TrimLevel;
            PrimaryErrorCode = diagnosticReport.PrimaryErrorCode;
            ErrorCodes = diagnosticReport.ErrorCodeList;
            SecondaryErrorCodes = diagnosticReport.SecondaryErrorCodeList;
            StoredErrorCodes = diagnosticReport.StoredErrorCodeList;
            PendingErrorCodes = diagnosticReport.PendingErrorCodeList;
            PermanentCodes = diagnosticReport.PermanentCodeList;
            SrsCurrentCodes = diagnosticReport.SRSCurrentCodeList;
            SrsHistoryCodes = diagnosticReport.SRSHistoryCodeList;
            Obd1CurrentCodes = diagnosticReport.OBD1CurrentCodeList;
            Obd1HistoryCodes = diagnosticReport.OBD1HistoryCodeList;
            AbsCurrentCodes = diagnosticReport.ABSCurrentCodeList;
            AbsHistoryCodes = diagnosticReport.ABSHistoryCodeList;

            Monitor = diagnosticReport.Monitor;
            FreezeFrame = diagnosticReport.FreezeFrame;

            ScheduledMaintenanceNextMileage = diagnosticReport.ScheduledMaintenanceNextMileage;
            UnScheduledMaintenanceNextMileage = diagnosticReport.UnScheduledMaintenanceNextMileage;
            RecallCount = diagnosticReport.RecallCount ?? 0;
            RecallItems = diagnosticReport.RecallItems;
            TsbCount = diagnosticReport.TsbCount ?? 0;

            ToolLedstatus = diagnosticReport.ToolLEDStatus ?? 0;
            ToolMilStatus = diagnosticReport.ToolMilStatus ?? 0;

            CreatedDateTimeUtc = DateTime.UtcNow;

            if (diagnosticReport.Errors != null && diagnosticReport.Errors.Any())
            {
                ReportDtc = diagnosticReport.Errors.Select(v => new ReportDtc(v)).ToList();

                if (diagnosticReport.Fixes != null && diagnosticReport.Fixes.Any())
                {
                    foreach (var dtc in ReportDtc)
                    {
                        var fixes = diagnosticReport.Fixes.Where(f => f.ErrorCode.Equals(dtc.Code, StringComparison.OrdinalIgnoreCase));
                        if (fixes != null && fixes.Any())
                        {
                            foreach (var fix in fixes)
                            {
                                var f = new ReportFix(fix);
                                f.ReportId = Id;
                                dtc.ReportFix.Add(f);
                            }
                        }
                    }
                }

            }

            if (diagnosticReport.Warranties != null && diagnosticReport.Warranties.Any())
            {
                ReportWarranty = diagnosticReport.Warranties.Select(p => new ReportWarranty(p)).ToList();
            }

            if (diagnosticReport.ScheduledMaintenances != null && diagnosticReport.ScheduledMaintenances.Any())
            {
                ReportScheduledMaintenance = diagnosticReport.ScheduledMaintenances.Select(v => new ReportScheduledMaintenance(v)).ToList();
            }

            if (diagnosticReport.UnScheduledMaintenanceServices != null && diagnosticReport.UnScheduledMaintenanceServices.Any())
            {
                ReportUnScheduledMaintenance = diagnosticReport.UnScheduledMaintenanceServices.Select(v => new ReportUnScheduledMaintenance(v)).ToList();
            }

            if (diagnosticReport.PredictiveDiagnostics != null && diagnosticReport.PredictiveDiagnostics.Any())
            {
                ReportPredictiveDiagnostic = diagnosticReport.PredictiveDiagnostics.Select(v => new ReportPredictiveDiagnostic(v)).ToList();
            }

            if (diagnosticReport.Recalls != null && diagnosticReport.Recalls.Any())
            {
                ReportRecall = diagnosticReport.Recalls.Select(v => new ReportRecall(v)).ToList();
            }
            ReportModule = new List<ReportModule>();
        }

        public ReportModel ToApiModelDto(string timeZone)
        {
            var target = new ReportModel
            {
                Id = Id,
                WorkOrderNumber = WorkOrderNumber,
                ReportNumber = ReportNumber,
                Vin = Vin,
                OriginalTirePressure = OriginalTirePressure,
                LFTire = Lftire,
                RFTire = Rftire,
                LRTire = Lrtire,
                RRTire = Rrtire,
                OilLevel = OilLevel,
                BrakePadLife = BrakePadLife,
                PrimaryErrorCode = PrimaryErrorCode,
                TirePressureUnit = TirePressureUnit,
                Mileage = Mileage,
                Status = Status,

                BatteryStatus = BatteryStatus,
                UrgencyOfRepairStatus = (int)RepairUrgencyStatus.Low,
                BatteryVoltageStatus = (int)BatteryVoltageStatus.Unknown,
                CheckEngineLightStatus = (int)CheckEngineLightStatus.Green
            };

            var engineType = string.IsNullOrWhiteSpace(EngineType) ? string.Empty : EngineType.Split(';').First();
            target.YMM = string.Format("{0} {1} {2}", Year, Make, Model);
            target.YMME = string.Format("{0} {1} {2} {3}", Year, Make, Model, engineType);
            target.Engine = engineType;

            var createdDate = CreatedDateTimeUtc.ConvertToTimeZone(timeZone);
            target.CreatedDate = createdDate.ToString("MM/dd/yy");
            target.CreatedTime = string.Format("{0:t}", createdDate);
            target.TimeAgo = CreatedDateTimeUtc.GetTimeAgo();

            if (ReportModule != null && ReportModule.Any())
            {
                target.ModulesScannedCount = ReportModule.Count();

                List<string> errors = new List<string>();

                var modulesWithDTC = new List<Data.Dto.ReportModule>();
                var modulesWithoutDTC = new List<Data.Dto.ReportModule>();

                foreach (var module in ReportModule)
                {
                    var reportModule = new Data.Dto.ReportModule
                    {
                        Id = module.Id,
                        FullName = module.ModuleName,
                        System = module.System,
                        SubSystem = module.SubSystem,
                    };

                    var moduleDTCs = new List<ReportModuleDTC>();

                    if (module.ReportModuleDtc != null && module.ReportModuleDtc.Any())
                    {
                        foreach (var dtc in module.ReportModuleDtc)
                        {
                            if (!string.IsNullOrWhiteSpace(dtc.Code) &&
                                !moduleDTCs.Any(v => dtc.Code.Equals(v.Code, StringComparison.OrdinalIgnoreCase) && dtc.Type == v.Type))
                            {
                                var moduleItem = new ReportModuleDTC
                                {
                                    Code = dtc.Code,
                                    Definition = dtc.Def,
                                    Type = dtc.Type
                                };
                                moduleDTCs.Add(moduleItem);
                                errors.Add(dtc.Code);
                            }
                        }
                        reportModule.dtcs = moduleDTCs;
                        reportModule.DTCCount = moduleDTCs.Count();

                        modulesWithDTC.Add(reportModule);
                    }
                    else
                    {
                        modulesWithoutDTC.Add(reportModule);
                    }
                }
                if (errors.Any())
                {
                    target.UrgencyOfRepairStatus = (int)RepairUrgencyStatus.Medium;
                    target.CheckEngineLightStatus = (int)CheckEngineLightStatus.Yellow;
                }

                target.ModulesWithDTC = modulesWithDTC;
                target.ModulesWithoutDTC = modulesWithoutDTC;

                target.TotalDTCCount = errors.Count();
                target.TotalErrorCodeCount = errors.Count();

                if (!string.IsNullOrEmpty(PrimaryErrorCode) && !errors.Contains(PrimaryErrorCode.Trim()))
                {
                    target.TotalErrorCodeCount = target.TotalErrorCodeCount + 1;
                }
            }

            if (!string.IsNullOrEmpty(PrimaryErrorCode))
            {
                target.UrgencyOfRepairStatus = (int)RepairUrgencyStatus.High;
                target.CheckEngineLightStatus = (int)CheckEngineLightStatus.Red;
            }

            if (CurrentBatteryVoltage.HasValue)
            {
                var vol = MathExtension.TruncateToDecimalPlace(CurrentBatteryVoltage.Value / 1000, 2);

                if (vol >= 12.5m)
                {
                    target.BatteryVoltageStatus = (int)BatteryVoltageStatus.Green;
                }
                else if (vol >= 12.1m)
                {
                    target.BatteryVoltageStatus = (int)BatteryVoltageStatus.Yellow;
                }
                else
                {
                    target.BatteryVoltageStatus = (int)BatteryVoltageStatus.Red;
                }
            }

            return target;
        }

        public ReportHistory ToApiHistoryModelDto(string timeZone)
        {
            var target = new ReportHistory
            {
                Id = Id,
                WorkOrderNumber = WorkOrderNumber,
                ReportNumber = ReportNumber,
                Vin = Vin,
                Mileage = Mileage,
                CreatedDateTimeUtc = CreatedDateTimeUtc.ToString("yyyy-MM-ddTHH:mm:ss"),
                Status = Status
            };

            var engineType = string.IsNullOrWhiteSpace(EngineType) ? string.Empty : EngineType.Split(';').First();
            target.YMM = string.Format("{0} {1} {2}", Year, Make, Model);
            target.YMME = string.Format("{0} {1} {2} {3}", Year, Make, Model, engineType);
            target.Engine = engineType;

            var createdDate = CreatedDateTimeUtc.ConvertToTimeZone(timeZone);
            target.CreatedDate = createdDate.ToString("MM/dd/yy");
            target.CreatedTime = string.Format("{0:t}", createdDate);
            target.CreatedDateTime = createdDate.ToString("yyyy-MM-ddTHH:mm:ss");
            target.TimeAgo = CreatedDateTimeUtc.GetTimeAgo();

            return target;
        }


        public void Map()
        {
            Monitors = ToolUtility.Convert2Object<MonitorInfo>(MONITORS, Monitor) ?? new List<MonitorInfo>();
            FreezeFrames = ToolUtility.Convert2Object<FreezeFrameInfo>(FREEZE_FRAMES, FreezeFrame) ?? new List<FreezeFrameInfo>();
        }
    }
}
