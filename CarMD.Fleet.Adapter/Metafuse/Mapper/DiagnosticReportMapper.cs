using MetafuseReference;
using CarMD.Fleet.Core.Mapper;
using System;
using Innova.Utilities.Shared.Enums;
using System.Linq;
using CarMD.Fleet.Common.Enum;
using System.Collections.Generic;
using CarMD.Fleet.Data.Dto.Metafuse;

namespace CarMD.Fleet.Adapter.Metafuse.Mapper
{
    internal sealed class DiagnosticReportMapper : BaseMapper<DiagReportInfo, DiagnosticReport>
    {
        public override void Map(DiagReportInfo source, DiagnosticReport target)
        {
            var vehicle = source.Vehicle;
            if (vehicle != null)
            {
                target.Vin = vehicle.VIN;
                target.CreatedDateTimeUtc = DateTime.UtcNow;
                target.DiagnosticReportId = source.DiagnosticReportId;

                //Map vehicle
                target.Aaia = vehicle.AAIA;
                target.EngineType = vehicle.EngineType;
                target.Make = vehicle.Make;
                target.Model = vehicle.Model;
                target.Year = vehicle.Year;
                target.TrimLevel = vehicle.TrimLevel;
                target.Transmission = vehicle.Transmission;
                target.Manufacture = vehicle.ManufacturerName;
                target.Mileage = vehicle.Mileage ?? 0;
                target.IsActive = vehicle.IsValid;
            }

            try
            {
                target.ToolLEDStatus = (int)(ToolLEDStatus)Enum.Parse(typeof(ToolLEDStatus), source.ToolLEDStatusDesc);
            }
            catch (Exception)
            {
                target.ToolLEDStatus = (int)ToolLEDStatus.Error;
            }

            #region Map DTCs
            if (source.Errors != null && source.Errors.Length > 0)
                source.Errors = source.Errors.Where(e => !string.IsNullOrEmpty(e.Code)).ToArray();

            target.Errors = MetafuseMapper.MapDtcs(source.Errors);
            MapErrorsToCodeLists(target);

            target.Fixes = MetafuseMapper.MapFixes(source.Fixes);
            #endregion

            #region Map Waranty
            if (source.VehicleWarrantyDetails != null && source.VehicleWarrantyDetails.Any())
            {

                target.Warranties = MetafuseMapper.MapWarranties(source.VehicleWarrantyDetails);
            }
            #endregion

            #region Map Scheduled Maintenances
            target.ScheduledMaintenanceNextMileage = source.ScheduledMaintenanceNextMileage ?? 0;
            if (source.ScheduleMaintenanceServices != null && source.ScheduleMaintenanceServices.Any())
            {
                target.ScheduledMaintenances = MetafuseMapper.MapScheduleMaintenances(source.ScheduleMaintenanceServices);
            }
            #endregion

            #region Map UnScheduled Maintenances
            target.UnScheduledMaintenanceNextMileage = source.UnScheduledMaintenanceNextMileage ?? 0;
            if (source.UnScheduledMaintenanceServices != null && source.UnScheduledMaintenanceServices.Any())
            {
                target.UnScheduledMaintenanceServices = MetafuseMapper.MapScheduleMaintenances(source.UnScheduledMaintenanceServices);
            }
            #endregion

            #region Mapping Monitor
            if (source.Monitors != null && source.Monitors.Any())
            {
                foreach (var monitor in source.Monitors)
                {
                    if (!string.IsNullOrWhiteSpace(monitor.Description) && !string.IsNullOrWhiteSpace(monitor.Value))
                    {
                        target.Monitors.Add(new Innova.Utilities.Shared.Tool.MonitorInfo
                        {
                            Description = monitor.Description,
                            Value = monitor.Value
                        });

                        if (monitor.Description.EndsWith("MIL", StringComparison.OrdinalIgnoreCase) ||
                            monitor.Description.StartsWith("MIL ", StringComparison.OrdinalIgnoreCase) ||
                            monitor.Description.EndsWith("(MIL)", StringComparison.OrdinalIgnoreCase))
                        {
                            if (monitor.Value.EndsWith("On", StringComparison.OrdinalIgnoreCase) ||
                                monitor.Value.EndsWith("«ON»", StringComparison.OrdinalIgnoreCase) ||
                                monitor.Value.EndsWith("«ENCENDIDO»", StringComparison.OrdinalIgnoreCase) ||
                                monitor.Value.EndsWith("ENCENDIDO", StringComparison.OrdinalIgnoreCase)) //spanish
                            {
                                target.ToolMilStatus = (int)ToolMilStatus.On;
                            }
                            else
                            {
                                target.ToolMilStatus = (int)ToolMilStatus.Off;
                            }
                        }

                    }
                }
            }
            #endregion

            #region Mapping FreezeFrame
            if (source.FreezeFrame != null && source.FreezeFrame.Any())
            {
                target.FreezeFrames = source.FreezeFrame.Select(x => new Innova.Utilities.Shared.Tool.FreezeFrameInfo
                {
                    Description = x.Description,
                    Value = x.Value
                }).ToList();
            }
            #endregion

            #region Map Recall
            target.RecallCount = source.Recalls != null ? source.Recalls.Count() : 0;
            target.Recalls = MetafuseMapper.MapRecalls(source.Recalls);
            target.RecallItems = string.Join(",", target.Recalls.Select(v => v.CampaignNumber));

            #endregion

            #region Map TSB
            target.TsbCount = source.TSBCountAll.HasValue ? source.TSBCountAll.Value : 0;
            #endregion

            MapMonitor(source, target);
            MapFreezeFrame(source, target);

            target.Map();
        }

        public void MapErrorsToCodeLists(DiagnosticReport target)
        {
            var Errors = target.Errors;

            var primaryCode = string.Empty;
            var storedCodes = new List<string>();
            var errorCodes = new List<string>();
            var penddingCodes = new List<string>();
            var permanentCodes = new List<string>();
            var secondaryCodes = new List<string>();
            var absCurrentList = new List<string>();
            var absHistoryList = new List<string>();
            var srsCurrentList = new List<string>();
            var srsHistoryList = new List<string>();
            var obd1CurrentList = new List<string>();
            var obd1HistoryList = new List<string>();

            if (Errors != null)
            {
                foreach (var error in Errors)
                {
                    //add to all error code
                    if (!errorCodes.Contains(error.Code))
                    {
                        errorCodes.Add(error.Code);
                    }

                    switch (error.CodeType)
                    {
                        //PowerTrain
                        case ErrorCodeSystemType.Powertrain:
                            {
                                switch (error.CodeLevel)
                                {
                                    //primary code
                                    case ErrorCodeLevel.PrimaryErrorCode:
                                        primaryCode = error.Code;
                                        //add this primary code to stored code
                                        if (!storedCodes.Contains(error.Code))
                                        {
                                            storedCodes.Insert(0, error.Code);
                                        }
                                        break;
                                    //first stored code
                                    case ErrorCodeLevel.FirstStoredCode:
                                        if (!storedCodes.Contains(error.Code))
                                        {
                                            storedCodes.Insert(0, error.Code);
                                        }
                                        break;
                                    //addditional error code
                                    case ErrorCodeLevel.AdditionalStoredCode:
                                        if (!storedCodes.Contains(error.Code))
                                        {
                                            storedCodes.Add(error.Code);
                                        }
                                        break;
                                    //first pendding code
                                    case ErrorCodeLevel.FirstPendingCode:
                                        if (!penddingCodes.Contains(error.Code))
                                        {
                                            penddingCodes.Insert(0, error.Code);
                                        }
                                        break;
                                    //additional pendding code
                                    case ErrorCodeLevel.AdditionalPendingCode:
                                        if (!penddingCodes.Contains(error.Code))
                                        {
                                            penddingCodes.Add(error.Code);
                                        }
                                        break;
                                    //first permanent
                                    case ErrorCodeLevel.FirstPermanentCode:
                                        if (!permanentCodes.Contains(error.Code))
                                        {
                                            permanentCodes.Insert(0, error.Code);
                                        }
                                        break;
                                    //additional permanent
                                    case ErrorCodeLevel.AdditionalPermanentCode:
                                        if (!permanentCodes.Contains(error.Code))
                                        {
                                            permanentCodes.Add(error.Code);
                                        }
                                        break;
                                }
                            }
                            break;

                        //Obd1
                        case ErrorCodeSystemType.Obd1:
                            {
                                switch (error.CodeLevel)
                                {
                                    //primary code
                                    case ErrorCodeLevel.PrimaryErrorCode:
                                        if (!obd1CurrentList.Contains(error.Code))
                                        {
                                            obd1CurrentList.Insert(0, error.Code);
                                        }
                                        break;
                                    //first stored code
                                    case ErrorCodeLevel.FirstStoredCode:
                                        if (!obd1CurrentList.Contains(error.Code))
                                        {
                                            obd1CurrentList.Insert(0, error.Code);
                                        }
                                        break;
                                    //addditional error code
                                    case ErrorCodeLevel.AdditionalStoredCode:
                                        if (!obd1CurrentList.Contains(error.Code))
                                        {
                                            obd1CurrentList.Add(error.Code);
                                        }
                                        break;
                                    //first pendding code
                                    case ErrorCodeLevel.FirstPendingCode:
                                        if (!obd1HistoryList.Contains(error.Code))
                                        {
                                            obd1HistoryList.Insert(0, error.Code);
                                        }
                                        break;
                                    //additional pendding code
                                    case ErrorCodeLevel.AdditionalPendingCode:
                                        if (!obd1HistoryList.Contains(error.Code))
                                        {
                                            obd1HistoryList.Add(error.Code);
                                        }
                                        break;
                                }
                            }
                            break;

                        //Abs
                        case ErrorCodeSystemType.Abs:
                            {
                                switch (error.CodeLevel)
                                {
                                    //primary code
                                    case ErrorCodeLevel.PrimaryErrorCode:

                                        //add this primary code to stored code
                                        if (!absCurrentList.Contains(error.Code))
                                        {
                                            absCurrentList.Insert(0, error.Code);
                                        }
                                        break;
                                    //first stored code
                                    case ErrorCodeLevel.FirstStoredCode:
                                        if (!absCurrentList.Contains(error.Code))
                                        {
                                            absCurrentList.Insert(0, error.Code);
                                        }
                                        break;
                                    //addditional error code
                                    case ErrorCodeLevel.AdditionalStoredCode:
                                        if (!absCurrentList.Contains(error.Code))
                                        {
                                            absCurrentList.Add(error.Code);
                                        }
                                        break;
                                    //first pendding code
                                    case ErrorCodeLevel.FirstPendingCode:
                                        if (!absHistoryList.Contains(error.Code))
                                        {
                                            absHistoryList.Insert(0, error.Code);
                                        }
                                        break;
                                    //additional pendding code
                                    case ErrorCodeLevel.AdditionalPendingCode:
                                        if (!absHistoryList.Contains(error.Code))
                                        {
                                            absHistoryList.Add(error.Code);
                                        }
                                        break;
                                }
                            }
                            break;

                        //SRS
                        case ErrorCodeSystemType.Srs:
                            {
                                switch (error.CodeLevel)
                                {
                                    //primary code
                                    case ErrorCodeLevel.PrimaryErrorCode:
                                        if (!srsCurrentList.Contains(error.Code))
                                        {
                                            srsCurrentList.Insert(0, error.Code);
                                        }
                                        break;
                                    //first stored code
                                    case ErrorCodeLevel.FirstStoredCode:
                                        if (!srsCurrentList.Contains(error.Code))
                                        {
                                            srsCurrentList.Insert(0, error.Code);
                                        }
                                        break;
                                    //addditional error code
                                    case ErrorCodeLevel.AdditionalStoredCode:
                                        if (!srsCurrentList.Contains(error.Code))
                                        {
                                            srsCurrentList.Add(error.Code);
                                        }
                                        break;
                                    //first pendding code
                                    case ErrorCodeLevel.FirstPendingCode:
                                        if (!srsHistoryList.Contains(error.Code))
                                        {
                                            srsHistoryList.Insert(0, error.Code);
                                        }
                                        break;
                                    //additional pendding code
                                    case ErrorCodeLevel.AdditionalPendingCode:
                                        if (!srsHistoryList.Contains(error.Code))
                                        {
                                            srsHistoryList.Add(error.Code);
                                        }
                                        break;
                                }
                            }
                            break;
                    }
                }
            }
            //add to secondary code
            foreach (var error in errorCodes)
            {
                if (!primaryCode.Equals(error, StringComparison.OrdinalIgnoreCase))
                {
                    secondaryCodes.Add(error);
                }
            }

            target.PrimaryErrorCode = primaryCode;
            target.SecondaryErrorCodes = secondaryCodes;
            target.StoredErrorCodes = storedCodes;
            target.PendingErrorCodes = penddingCodes;
            target.PermanentCodes = permanentCodes;
            target.ErrorCodes = errorCodes;
            target.ABSCurrentCodes = absCurrentList;
            target.ABSHistoryCodes = absHistoryList;
            target.SRSCurrentCodes = srsCurrentList;
            target.SRSHistoryCodes = srsHistoryList;
            target.OBD1CurrentCodes = obd1CurrentList;
            target.OBD1HistoryCodes = obd1HistoryList;
        }

        private static void MapMonitor(DiagReportInfo metafuseReport, DiagnosticReport innovaReport)
        {
            var monitors = new List<Innova.Utilities.Shared.Tool.MonitorInfo>();
            if (metafuseReport.Monitors != null)
            {
                foreach (var monitor in metafuseReport.Monitors)
                {
                    var item = new Innova.Utilities.Shared.Tool.MonitorInfo
                    {
                        Description = monitor.Description,
                        Value = monitor.Value
                    };

                    monitors.Add(item);
                    if (!"MIL".EndsWith(monitor.Description, StringComparison.OrdinalIgnoreCase) || !"(MIL)".EndsWith(monitor.Description, StringComparison.OrdinalIgnoreCase)) continue;

                    if ("On".EndsWith(monitor.Value, StringComparison.OrdinalIgnoreCase) ||
                        "ENCENDIDO".EndsWith(monitor.Value, StringComparison.OrdinalIgnoreCase)) //spanish
                        innovaReport.ToolMilStatus = (int)ToolMilStatus.On;
                    else
                        innovaReport.ToolMilStatus = (int)ToolMilStatus.Off;
                }
            }
            innovaReport.Monitors = monitors;
        }

        private static void MapFreezeFrame(DiagReportInfo metafuseReport, DiagnosticReport innovaReport)
        {
            if (metafuseReport.FreezeFrame != null && metafuseReport.FreezeFrame.Any())
            {
                innovaReport.FreezeFrames = metafuseReport.FreezeFrame.Select(x => new Innova.Utilities.Shared.Tool.FreezeFrameInfo
                {
                    Description = x.Description,
                    Value = x.Value
                }).ToList();
            }
        }
    }
}
