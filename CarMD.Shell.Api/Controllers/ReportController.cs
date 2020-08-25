using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Core.Utility;
using CarMD.Fleet.Data.Dto;
using CarMD.Fleet.Data.Request.Api;
using CarMD.Fleet.Data.Response;
using CarMD.Fleet.Service.IService;
using CarMD.Fleet.Vehimatics.Api.Attributes;
using CarMD.Shell.Api.Attributes;
using CarMD.Shell.Api.Helpers;
using CarMD.Shell.Api.Models;
using Innova.Utilities.Shared.Tool;
using Innova2.VehicleDataLib.Enums.Device;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CarMD.Shell.Api.Controllers
{
    public class ReportController : BaseApiController
    {
        #region Constants

        const string MONITORS = "Monitors";
        const string FREEZE_FRAMES = "FreezeFrames";

        #endregion

        private static IVehicleService _vehicleService;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ReportController(IApiService apiService,
                        IVehicleService vehicleService) : base(apiService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost("Reports")]
        [ThrottleCreateReportFilter(10)]
        [ValidateInputFilter("CreateReport")]
        [RequestResponseSize("CreateReport")]
        public ContentResult Create([FromBody]VehicleDataModel vehicleDataModel)
        {
            // _logger.Fatal(string.Format("Monitor: {0} - FF: {1} - ECMDTCs: {2} - TCMDTCs: {3}", vehicleDataModel.MonitorStatusRaw, vehicleDataModel.FreezeFrameRaw, vehicleDataModel.ECMDTCs, vehicleDataModel.TCMDTCs));
            var validate = ValidateGetVehicleData(vehicleDataModel);

            if (validate.ResultCode != ResultCode.Ok)
            {
                return CreateJsonResponse(validate);
            }

            var result = _vehicleService.DecodeVin(vehicleDataModel.Vin);
            if (result.ResultCode == ResultCode.Ok)
            {
                var createReportRequest = new CreateReportRequest();

                var vehicle = result.Data;
                createReportRequest.UserId = UserId;
                createReportRequest.KioskId = KioskId;
                createReportRequest.TimeZone = TimeZone;
                createReportRequest.VehicleDataModel = vehicleDataModel;
                createReportRequest.Vehicle = vehicle;

                //Parse Data to Raw String 
                try
                {
                    vehicleDataModel.UsbProductId = vehicleDataModel.UsbProductId ?? (int)UsbProductId.InnovaPro13;
                    var rawData = RawV5Helper.GetRawDataString(vehicleDataModel);
                    createReportRequest.RawString = rawData;

                    //_logger.Fatal(string.Format("[CreateReport] Parse Raw String {0}", rawData));
                }
                catch (Exception ex)
                {
                    _logger.Fatal(string.Format("[CreateReport] Parse Raw String fail : Monitor: {0} - FF: {1} - ECMDTCs: {2} - TCMDTCs: {3} - ex: {4}", vehicleDataModel.MonitorStatusRaw, vehicleDataModel.FreezeFrameRaw, vehicleDataModel.ECMDTCs, vehicleDataModel.TCMDTCs, LogUtility.GetDetailsErrorMessage(ex)));
                    return CreateJsonResponse(new ServiceResult<VehicleDataModel>
                    {
                        ResultCode = ResultCode.DataInvaild,
                        Message = "Parse Data to Raw String fail.",
                        Data = null
                    });
                }

                var report = _apiService.CreateDiagnosticReport(createReportRequest);

                return CreateJsonResponse(report);
            }
            else
            {

                return CreateJsonResponse(result);
            }
        }

        [HttpPost("InnovaReports")]
        [ThrottleCreateReportFilter(10)]
        [ValidateInputFilter("CreateInnovaReport")]
        [RequestResponseSize("CreateInnovaReport")]
        public ContentResult CreateInnovaReport([FromBody]VehicleInnovaDataModel vehicleDataModel)
        {
            // _logger.Fatal(string.Format("Monitor: {0} - FF: {1} - ECMDTCs: {2} - TCMDTCs: {3}", vehicleDataModel.MonitorStatusRaw, vehicleDataModel.FreezeFrameRaw, vehicleDataModel.ECMDTCs, vehicleDataModel.TCMDTCs));
            var validate = ValidateToCreateReport(vehicleDataModel);

            if (validate.ResultCode != ResultCode.Ok)
            {
                return CreateJsonResponse(validate);
            }

            var result = _vehicleService.DecodeVin(vehicleDataModel.Vin);
            if (result.ResultCode == ResultCode.Ok)
            {
                var createReportRequest = new CreateInnovaReportRequest();

                var vehicle = result.Data;

                createReportRequest.UserId = UserId;
                createReportRequest.KioskId = KioskId;
                createReportRequest.TimeZone = TimeZone;
                createReportRequest.VehicleInnovaDataModel = vehicleDataModel;
                createReportRequest.Vehicle = vehicle;

                //Parse Data to Raw String 
                try
                {
                    #region Check to decode the raw data
                    var rawData = ReportHelper.GetRawData(vehicleDataModel);
                    if (string.IsNullOrWhiteSpace(rawData))
                    {
                        return CreateJsonResponse(new ServiceResult<VehicleDataModel>
                        {
                            ResultCode = ResultCode.DataInvaild,
                            Message = "Invalid Data Buffer.",
                            Data = null
                        });
                    }
                    #endregion
                    createReportRequest.RawString = rawData;

                    //_logger.Fatal(string.Format("[CreateReport] Parse Raw String {0}", rawData));
                }
                catch (Exception ex)
                {
                    _logger.Fatal(string.Format("[CreateReport] Parse Raw String fail : Monitor: {0} - FF: {1} - ECMDTCs: {2} - TCMDTCs: {3} - ex: {4}", vehicleDataModel.MonitorStatusEcmRaw + ";" + vehicleDataModel.MonitorStatusTcmRaw, vehicleDataModel.FreezeFrameEcmRaw + ";" + vehicleDataModel.FreezeFrameTcmRaw, vehicleDataModel.DtcsEcmRaw, vehicleDataModel.DtcsTcmRaw, LogUtility.GetDetailsErrorMessage(ex)));
                    return CreateJsonResponse(new ServiceResult<VehicleDataModel>
                    {
                        ResultCode = ResultCode.DataInvaild,
                        Message = "Parse Data to Raw String fail.",
                        Data = null
                    });
                }

                var report = _apiService.CreateInnovaDiagnosticReport(createReportRequest);

                return CreateJsonResponse(report);
            }
            else
            {

                return CreateJsonResponse(result);
            }
        }

        [HttpGet("Reports")]
        [ValidateInputFilter("SearchReports")]
        public ContentResult Search([FromQuery] int pageSize = 20, [FromQuery] int pageIndex = 1)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 20 : pageSize;
            var result = _apiService.SearchReports(KioskId, TimeZone, pageSize, pageIndex);

            return CreateJsonResponse(result);
        }

        [HttpGet("Reports/{reportId}")]
        [ValidateInputFilter("GetReport")]
        public ContentResult GetReport(Guid reportId)
        {
            var result = _apiService.GetReportById(KioskId, TimeZone, reportId);
            return CreateJsonResponse(result);
        }

        [HttpGet("Vehicles/Decode/{vin}")]
        [ValidateInputFilter("DecodeVin")]
        [RequestResponseSize("DecodeVin")]
        public ContentResult DecodeVin(string vin)
        {
            var result = _vehicleService.DecodeVin(vin);
            return CreateJsonResponse(result);
        }

        [HttpGet("Makes/{year?}")]
        [ValidateInputFilter("Makes")]
        [RequestResponseSize("Makes")]
        public ContentResult Makes(int year = 0)
        {
            var result = _vehicleService.Makes(year);
            return CreateJsonResponse(result);
        }

        [HttpGet("Years/{make?}")]
        [ValidateInputFilter("Years")]
        [RequestResponseSize("Years")]
        public ContentResult Years(string make = "")
        {
            var result = _vehicleService.Years(make);
            return CreateJsonResponse(result);
        }

        [HttpGet("Models/{make}/{year}")]
        [ValidateInputFilter("Models")]
        [RequestResponseSize("Models")]
        public ContentResult Models(string make, int year)
        {
            var result = _vehicleService.Models(make, year);
            return CreateJsonResponse(result);
        }

        [HttpGet("DLC/{make}/{year}/{model}")]
        [ValidateInputFilter("DLC")]
        [RequestResponseSize("DLC")]
        public ContentResult DLC(string make, int year, string model, bool includeReport = false)
        {
            var timeZone = includeReport == true ? TimeZone : string.Empty;
            var result = _vehicleService.DLC(make, year, model, timeZone);
            return CreateJsonResponse(result);
        }

        [HttpGet("DLC/{vin}")]
        [ValidateInputFilter("DLCByVin")]
        public ContentResult DLCByVin(string vin, bool includeReport = false)
        {
            var timeZone = includeReport == true ? TimeZone : string.Empty;
            var result = _vehicleService.DLCByVin(vin, timeZone);
            return CreateJsonResponse(result);
        }

        [HttpPost("DecodeRaw")]
        [ValidateAppFilter("DecodeRaw")]
        public ContentResult DecodeRaw([FromBody] DecodeRaw decodeRaw)
        {
            var result = new DecodeRawResponse();
            var message = string.Empty;
            if (!string.IsNullOrWhiteSpace(decodeRaw.MonitorStatusRaw))
            {
                try
                {
                    var data = new List<MonitorInfo>();
                    var monitors = MonitorHelper.ParseMonitor(decodeRaw.MonitorStatusRaw);
                    if (monitors != null && monitors.Any())
                    {
                        foreach (var monitor in monitors)
                        {
                            data.Add(new MonitorInfo
                            {
                                Description = monitor.Name,
                                Value = monitor.Key.ToString()
                            });
                        }
                    }
                    result.Monitors = data;
                    message = "Decode Monitor Raw Succeed - ";
                }
                catch (Exception ex)
                {
                    message = "Parse Monitor fail: " + LogUtility.GetDetailsErrorMessage(ex) + " - ";
                    _logger.Fatal(string.Format("[DecodeRaw] Parse Monitor fail : {0} - {1}", decodeRaw.MonitorStatusRaw, LogUtility.GetDetailsErrorMessage(ex)));
                }
            }
            else
            {
                message = "No MonitorStatusRaw found - ";
            }
            if (!string.IsNullOrWhiteSpace(decodeRaw.FreezeFrameRaw))
            {
                try
                {
                    var targets = new List<FreezeFrameInfo>();
                    string milDtc = string.Empty;
                    var ffs = FreezeFrameHelper.ParseFreezeFrame(decodeRaw.FreezeFrameRaw, out milDtc);
                    if (!string.IsNullOrWhiteSpace(milDtc))
                    {
                        targets.Add(new FreezeFrameInfo
                        {
                            Description = "MIL DTC",
                            Value = milDtc
                        });
                    }

                    foreach (var ff in ffs)
                    {
                        var value = !string.IsNullOrWhiteSpace(ff.Value) ? ff.Value : "N/A";
                        targets.Add(new FreezeFrameInfo
                        {
                            Description = ff.Description,
                            Value = value
                        });
                    }

                    result.FreezeFrames = targets;
                    message = message + "Decode FreezeFrame Raw Succeed.";
                }
                catch (Exception ex)
                {
                    message = message + "Parse FreezeFrame fail: " + LogUtility.GetDetailsErrorMessage(ex);
                    _logger.Fatal(string.Format("[DecodeRaw] Parse FreezeFrame fail : {0} - {1}", decodeRaw.FreezeFrameRaw, LogUtility.GetDetailsErrorMessage(ex)));
                }

            }
            else
            {
                message = message + "No FreezeFrameRaw found.";
            }

            return CreateJsonResponse(new ServiceResult<DecodeRawResponse>
            {
                ResultCode = ResultCode.Ok,
                Message = message,
                Data = result
            });
        }

        [HttpPost("Reports/EraseModuleDtcs")]
        [ValidateInputFilter("EraseModuleDtcs")]
        public ContentResult EraseModuleDtcs([FromBody] EraseModuleDtcModel model)
        {
            var result = _apiService.EraseModuleDtcs(KioskId, model);

            return CreateJsonResponse(result);
        }

        [HttpPost("Reports/SendEmail")]
        [ValidateInputFilter("SendEmail")]
        public ContentResult SendEmail([FromBody] ReportEmail email)
        {
            var result = _apiService.SendReportEmail(email.ReportId, KioskId, email.Email);
            return CreateJsonResponse(result);
        }


        #region Create RawData

        private ServiceResult<VehicleDataModel> ValidateGetVehicleData(VehicleDataModel vehicleDataModel)
        {
            var resultCode = ResultCode.Ok;
            var message = string.Empty;
            //if (!System.Enum.IsDefined(typeof(UsbProductIdLib2), vehicleDataModel.UsbProductId))
            //{
            //    resultCode = ResultCode.E015018;
            //    message = "UsbProductId is invalid.";
            //}
            //if (!ToolsUseLib5.Contains(vehicleDataModel.UsbProductId))
            //{
            //    resultCode = ResultCode.E015019;
            //    message = "UsbProductId not supported.";
            //}

            return new ServiceResult<VehicleDataModel>
            {
                ResultCode = resultCode,
                Message = message,
                Data = vehicleDataModel
            };
        }

        private ServiceResult<VehicleInnovaDataModel> ValidateToCreateReport(VehicleInnovaDataModel vehicleDataModel)
        {
            //if (string.IsNullOrWhiteSpace(vehicleDataModel.Vin) || vehicleDataModel.Vin.Length != 17)
            //    return JsonResponse(HttpStatusCode.OK, new HttpMessage(MessageType.VIN_IS_REQUIRED, MessageType.VIN_IS_REQUIRED.ToName()));

            //if (vehicleDataModel.Mileage < 1)
            //    return JsonResponse(HttpStatusCode.OK, new HttpMessage(MessageType.INVALID_MILEAGE, MessageType.INVALID_MILEAGE.ToName()));

            //if (string.IsNullOrWhiteSpace(vehicleDataModel.DongleId))
            //    return JsonResponse(HttpStatusCode.OK, new HttpMessage(MessageType.INVALID_CHIPID, MessageType.INVALID_CHIPID.ToName()));

            //if (vehicleDataModel.UsbProductId < 1)
            //    return JsonResponse(HttpStatusCode.OK, new HttpMessage(MessageType.INVALID_USB_PRODUCTID, MessageType.INVALID_USB_PRODUCTID.ToName()));

            //if (vehicleDataModel.ScanningType.HasValue && !Enum.IsDefined(typeof(ScanningType), vehicleDataModel.ScanningType.Value))
            //    return JsonResponse(HttpStatusCode.OK, new HttpMessage(MessageType.INVALID_SCANNINGTYPE, MessageType.INVALID_SCANNINGTYPE.ToName()));

            //return null;

            var resultCode = ResultCode.Ok;
            var message = string.Empty;

            return new ServiceResult<VehicleInnovaDataModel>
            {
                ResultCode = resultCode,
                Message = message,
                Data = vehicleDataModel
            };
        }
        #endregion
    }
}