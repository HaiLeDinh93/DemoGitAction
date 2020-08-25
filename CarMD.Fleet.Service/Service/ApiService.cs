using CarMD.Fleet.Service.IService;
using CarMD.Fleet.Data.Response;
using CarMD.Fleet.Data.Response.Api;
using CarMD.Fleet.Common.Enum;
using log4net;
using System.Reflection;
using CarMD.Fleet.Repository.IRepository;
using CarMD.Fleet.Data.Request.Api;
using CarMD.Fleet.Core.Cryptography;
using System;
using CarMD.Fleet.Core.Utility;
using CarMD.Fleet.Data.Request;
using System.Collections.Generic;
using CarMD.Fleet.Data.EntityFramework;
using System.Linq;
using Mandrill;
using CarMD.Fleet.Common.Configuration;
using CarMD.Fleet.Service.Mandrill;
using CarMD.Fleet.Data.Dto;
using CarMD.Fleet.Adapter.Metafuse;
using CarMD.Fleet.Adapter.Metafuse.Model;
using CarMD.Fleet.Common.Helper;
using CarMD.Fleet.Common.MandrillApi;
using CarMD.Fleet.Common.Helpers;

namespace CarMD.Fleet.Service.Service
{
    public class ApiService : IApiService
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IMetafuseAdapter _metafuseAdapter;
        private readonly IUserRepository _userRepository;
        private readonly ILoggingErrorRepository _loggingErrorRepository;
        private readonly ILoggingTimeRepository _loggingTimeRepository;
        private readonly IFeedBackRepository _feedBackRepository;
        private readonly IKioskRepository _kioskRepository;
        private readonly IDiagnosticReportRepository _diagnosticReportRepository;
        private readonly IConsumerRepository _consumerRepository;

        public ApiService(IMetafuseAdapter metafuseAdapter,
            IUserRepository userRepository,
                         ILoggingErrorRepository loggingErrorRepository,
             IFeedBackRepository feedBackRepository,
             ILoggingTimeRepository loggingTimeRepository,
              IKioskRepository kioskRepository,
              IDiagnosticReportRepository diagnosticReportRepository,
              IConsumerRepository consumerRepository
              )
        {
            _metafuseAdapter = metafuseAdapter;
            _userRepository = userRepository;
            _loggingErrorRepository = loggingErrorRepository;
            _loggingTimeRepository = loggingTimeRepository;
            _feedBackRepository = feedBackRepository;
            _kioskRepository = kioskRepository;
            _diagnosticReportRepository = diagnosticReportRepository;
            _consumerRepository = consumerRepository;
        }

        public ServiceResult<bool> CreateLoggingError(Guid userId, long kioskId, LoggingErrorModel loggingErrorModel)
        {
            try
            {
                var model = new LoggingError(loggingErrorModel);
                model.UserId = userId;
                model.KioskId = kioskId;

                _loggingErrorRepository.Create(model);

                return new ServiceResult<bool>
                {
                    Data = true,
                    ResultCode = ResultCode.Ok,
                    Message = "Logging error success."
                };
            }
            catch (Exception ex)
            {
                _logger.Fatal(string.Format("[CreateLoggingError] Create Logging Error fail : {0}", LogUtility.GetDetailsErrorMessage(ex)));
                return new ServiceResult<bool>
                {
                    ResultCode = ResultCode.Error,
                    Message = "Logging error fail."
                };
            }
        }

        public ServiceResult<bool> CreateLoggingTime(Guid userId, long kioskId, LoggingTimeModel logTimeModel)
        {
            try
            {
                var model = new LoggingTime(logTimeModel);
                model.UserId = userId;
                model.KioskId = kioskId;

                _loggingTimeRepository.Create(model);

                return new ServiceResult<bool>
                {
                    Data = true,
                    ResultCode = ResultCode.Ok,
                    Message = "Logging time success."
                };
            }
            catch (Exception ex)
            {
                _logger.Fatal(string.Format("[CreateLoggingTime] Create Logging Time fail : {0}", LogUtility.GetDetailsErrorMessage(ex)));
                return new ServiceResult<bool>
                {
                    ResultCode = ResultCode.Error,
                    Message = "Logging time fail."
                };
            }
        }

        public ServiceResult<bool> FeedBack(FeedBackModel model)
        {
            var entity = new FeedBack
            {
                Id = Guid.NewGuid(),
                KioskId = model.KioskId,
                UserId = model.UserId,
                CustomerEmail = model.CustomerEmail,
                Type = model.Type,
                Comment = model.Comment,
                LikeThis = model.LikeThis,
                ReportId = model.ReportId,
                CreatedDate = DateTime.UtcNow
            };

            _feedBackRepository.Create(entity);

            var toList = new List<EmailAddress>();
            foreach (var item in model.ToEmails)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    toList.Add(new EmailAddress { email = item, name = "" });
                }
            }
            var isTesting = !string.IsNullOrEmpty(model.UserEmail) &&
                (model.UserEmail.ToLower().Contains("@carmd.com") || model.UserEmail.ToLower().Contains("@innova.com"));
            SendFeedBackEmailToAdmin(toList, model, isTesting);

            return new ServiceResult<bool>
            {
                ResultCode = ResultCode.Ok,
                Data = true
            };
        }

        public ServiceResult<bool> NotifyHavingDongle(long kioskId)
        {
            try
            {
                var feedBacks = _feedBackRepository.GetDongleNotify(kioskId);

                if (feedBacks != null && feedBacks.Any())
                {
                    var kiosk = _kioskRepository.GetFirstOrDefault(k => k.Id == kioskId);

                    if (kiosk == null)
                    {
                        _logger.Error(string.Format("Kiosk {0} not found.", kioskId));
                        return new ServiceResult<bool>
                        {
                            ResultCode = ResultCode.Error,
                            Data = false,
                            Message = "Kiosk not found"
                        };
                    }
                    var address = string.Format("Kiosk {0}", kiosk.Id.ToString().PadLeft(6, '0'));
                    if (!string.IsNullOrEmpty(kiosk.Address))
                    {
                        address += string.Format(" - {0},", kiosk.Address);
                    }
                    if (!string.IsNullOrEmpty(kiosk.City))
                    {
                        address += string.Format(" {0},", kiosk.City);
                    }
                    if (!string.IsNullOrEmpty(kiosk.State))
                    {
                        address += string.Format(" {0},", kiosk.State);
                    }
                    if (!string.IsNullOrEmpty(kiosk.PostalCode))
                    {
                        address += string.Format(" {0}", kiosk.PostalCode);
                    }

                    var result = SendMandrillEmailHelper.SendNotifyHavingDongle(feedBacks.Select(v => v.CustomerEmail).ToList(), address);

                    _feedBackRepository.UpdateDongleNotify(kioskId);
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(string.Format("[NotifyHavingDongle] Notify Having Dongle error : {0}", LogUtility.GetDetailsErrorMessage(ex)));
                return new ServiceResult<bool>
                {
                    ResultCode = ResultCode.Error,
                    Message = "Notify Having Dongle fail."
                };
            }

            return new ServiceResult<bool>
            {
                ResultCode = ResultCode.Ok,
                Data = true
            };
        }

        public ServiceResult<bool> EraseModuleDtcs(long kioskId, EraseModuleDtcModel model)
        {
            return _diagnosticReportRepository.EraseModuleDtcs(kioskId, model);
        }

        public ServiceResult<bool> SendReportEmail(Guid reportId, long kioskId, string email)
        {
            try
            {
                var report = _diagnosticReportRepository.GetFirstOrDefault(v => v.Id == reportId && v.KioskId == kioskId,
                        v => v.ReportDtc.Select(r => r.ReportDtcDefinition),
                        v => v.ReportModule.Select(r => r.ReportModuleDtc),
                        v => v.ReportFix.Select(r => r.ReportFixPart),
                        v => v.ReportScheduledMaintenance.Select(r => r.ReportScheduledMaintenanceFixPart),
                        v => v.ReportPredictiveDiagnostic.Select(r => r.ReportPredictiveDiagnosticFixPart)
                    );

                if (report == null)
                {
                    return new ServiceResult<bool>
                    {
                        ResultCode = ResultCode.E003000,
                        Data = false,
                        Message = "Report Not Found",
                    };
                }

                /// Create Consumer Vehicle 
                var consumer = _consumerRepository.GetFirstOrDefault(v => v.Email.ToLower() == email.ToLower(), v => v.Vehicle);
                if (consumer != null)
                {
                    report.ConsumerId = consumer.Id;
                    var vh = consumer.Vehicle.FirstOrDefault(v => v.Vin.Equals(report.Vin, StringComparison.OrdinalIgnoreCase));
                    if (vh == null)
                    {
                        vh = new Vehicle(report);
                        vh.ConsumerId = consumer.Id;
                        consumer.Vehicle.Add(vh);
                    }
                    else
                    {
                        vh.IsActive = true;
                        if (vh.Mileage < report.Mileage)
                        {
                            vh.Mileage = report.Mileage;
                        }
                    }
                }

                report.Email = email;

                var reportUrl = UrlHelper.Merge(CommonConfiguration.ShellConsumerRootUrl, string.Format("{0}?reportId={1}&email={2}", "Home/Consumer", reportId, email));

                var emailModel = report.ToEmailModel(reportUrl);

                var sendMail = SendMandrillEmailHelper.SendConsumerReportEmail(emailModel);

                if (!string.IsNullOrEmpty(sendMail))
                {
                    _logger.Error(string.Format("Send email to customer error {0}", sendMail));
                    return new ServiceResult<bool>
                    {
                        ResultCode = ResultCode.Error,
                        Data = false,
                        Message = sendMail
                    };
                }

                _diagnosticReportRepository.Update(report);

                return new ServiceResult<bool>
                {
                    ResultCode = ResultCode.Ok,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.FatalFormat("Send Email Error - {0}", LogUtility.GetDetailsErrorMessage(ex));
                return new ServiceResult<bool>
                {
                    ResultCode = ResultCode.Error,
                    Data = false,
                    Message = LogUtility.GetDetailsErrorMessage(ex),
                };
            }
        }

        public ServiceResult<UserModel> GetUserInformation(Guid userId)
        {
            var user = _userRepository.GetFirstOrDefault(u => u.Id == userId, u => u.Kiosk);

            if (user == null || !user.IsActive)
            {
                return new ServiceResult<UserModel>
                {
                    ResultCode = ResultCode.E002000,
                    Message = "User not found."
                };
            }

            return new ServiceResult<UserModel>
            {
                ResultCode = ResultCode.Ok,
                Message = "Get User Information succeed.",
                Data = user.ToApiDto()
            };
        }

        public ServiceResult<UserModel> Login(LoginModel authorizationModel, bool hashPassword = true)
        {
            var user = _userRepository.GetUser(authorizationModel.U);

            if (user == null || !user.IsActive)
            {
                return new ServiceResult<UserModel>
                {
                    ResultCode = ResultCode.E002000,
                    Message = "User not found."
                };
            }

            var password = authorizationModel.P;

            if (hashPassword)
            {
                password = SecurityPassword.HashPassword(authorizationModel.P);
            }

            if (!user.HashPassword.Equals(password))
            {
                return new ServiceResult<UserModel>
                {
                    ResultCode = ResultCode.E002002,
                    Message = "Invalid User ID and/or Password."
                };
            }

            var resultData = user.ToApiDto();

            return new ServiceResult<UserModel>
            {
                ResultCode = ResultCode.Ok,
                Message = "Login succeed.",
                Data = resultData
            };
        }

        public ServiceResult<LoggingErrorSearchModel> SearchLoggingError(SearchCriteria model)
        {
            var searchResult = _loggingErrorRepository.Search(model);

            var result = new LoggingErrorSearchModel
            {
                CurrentPage = searchResult.CurrentPage,
                PageCount = searchResult.PageCount,
                PageSize = searchResult.PageSize,
                RowCount = searchResult.RowCount,
                Logs = searchResult.Data != null ? searchResult.Data.ToList() : new List<LoggingError>()

            };

            return new ServiceResult<LoggingErrorSearchModel>
            {
                ResultCode = ResultCode.Ok,
                Message = "Search Diagnostic Report History succeed.",
                Data = result
            };
        }

        public ServiceResult<LoggingTimeSearchModel> SearchLoggingTime(SearchCriteria model)
        {
            var searchResult = _loggingTimeRepository.Search(model);

            var result = new LoggingTimeSearchModel
            {
                CurrentPage = searchResult.CurrentPage,
                PageCount = searchResult.PageCount,
                PageSize = searchResult.PageSize,
                RowCount = searchResult.RowCount,
                Logs = searchResult.Data != null ? searchResult.Data.ToList() : new List<LoggingTime>()
            };
            return new ServiceResult<LoggingTimeSearchModel>
            {
                ResultCode = ResultCode.Ok,
                Message = "Search Diagnostic Report History succeed.",
                Data = result
            };

        }

        public ServiceResult<ReportModel> CreateDiagnosticReport(Data.Request.Api.CreateReportRequest request)
        {
            try
            {
                var vehicleDataModel = request.VehicleDataModel;
                var vehicle = request.Vehicle;
                request.MilDTC = !string.IsNullOrWhiteSpace(request.MilDTC) ? request.MilDTC : "";

                var reportRequest = CreateReportRequest(vehicle, vehicleDataModel.Mileage, request.RawString);
                var errorMessage = string.Empty;
                var report = _metafuseAdapter.CreateTheDiagnosticReport(reportRequest, out errorMessage);

                if (report == null)
                {
                    _logger.Fatal(string.Format("[CreateReport] Create Diagnostic Report fail : Monitor: {0} - FF: {1} - ECMDTCs: {2} - TCMDTCs: {3} - Raw: {4} - error: {5}", vehicleDataModel.MonitorStatusRaw, vehicleDataModel.FreezeFrameRaw, vehicleDataModel.ECMDTCs, vehicleDataModel.TCMDTCs, request.RawString, errorMessage));
                    return new ServiceResult<ReportModel>
                    {
                        ResultCode = ResultCode.E003004,
                        Message = errorMessage,
                    };
                }

                var diagnosticReport = new Report(report);

                //Get 5 Year Cost To Own
                var vehicleRequest = CreateVehicleBaseRequest(vehicle, vehicleDataModel.Mileage);
                var info = _metafuseAdapter.Get5YearCostToOwnInfo(vehicleRequest);
                if (info != null)
                {
                    diagnosticReport.DepreciationCost = info.DepreciationCost;
                    diagnosticReport.FuelCost = info.FuelCost;
                    diagnosticReport.RepairCost = info.RepairCost;
                    diagnosticReport.InsuranceCost = info.InsuranceCost;
                    diagnosticReport.MaintenanceCost = info.MaintenanceCost;
                    diagnosticReport.TotalCostToOwn = info.TotalCostToOwn;
                }

                diagnosticReport.UserId = request.UserId;
                diagnosticReport.KioskId = request.KioskId;

                diagnosticReport.WorkOrderNumber = vehicleDataModel.WorkOrderNumber;
                diagnosticReport.Imei = vehicleDataModel.Imei;
                diagnosticReport.Mileage = vehicleDataModel.Mileage;
                diagnosticReport.Vin = vehicleDataModel.Vin;
                diagnosticReport.ToolMilStatus = request.ToolMilStatus;

                diagnosticReport.OriginalTirePressure = vehicleDataModel.OriginalTirePressure;
                diagnosticReport.Lftire = vehicleDataModel.LFTire;
                diagnosticReport.Rftire = vehicleDataModel.RFTire;
                diagnosticReport.Lrtire = vehicleDataModel.LRTire;
                diagnosticReport.Rrtire = vehicleDataModel.RRTire;
                diagnosticReport.TirePressureUnit = vehicleDataModel.TirePressureUnit;

                diagnosticReport.OriginalBatteryVoltage = vehicleDataModel.OriginalBatteryVoltage;
                diagnosticReport.CurrentBatteryVoltage = vehicleDataModel.CurrentBatteryVoltage;

                diagnosticReport.OilLevel = vehicleDataModel.OilLevel;
                diagnosticReport.BrakePadLife = vehicleDataModel.BrakePadLife;

                diagnosticReport.MonitorStatusRaw = vehicleDataModel.MonitorStatusRaw;
                diagnosticReport.FreezeFrameRaw = vehicleDataModel.FreezeFrameRaw;
                diagnosticReport.EcmdtcsRaw = vehicleDataModel.ECMDTCs;
                diagnosticReport.TcmdtcsRaw = vehicleDataModel.TCMDTCs;
                diagnosticReport.RawString = request.RawString;

                diagnosticReport.IsVinMismatch = vehicleDataModel.IsVinMismatch;
                diagnosticReport.ToolFirmware = vehicleDataModel.ToolFirmware;
                diagnosticReport.Status = vehicleDataModel.Status;
                diagnosticReport.BatteryStatus = vehicleDataModel.BatteryStatus;

                diagnosticReport.FirstName = vehicleDataModel.FirstName;
                diagnosticReport.LastName = vehicleDataModel.LastName;
                diagnosticReport.Email = vehicleDataModel.Email;
                diagnosticReport.Phone = vehicleDataModel.Phone;

                diagnosticReport.VehicleImageFileUrl = vehicle.ModelImageFileUrl;

                if (!string.IsNullOrEmpty(vehicleDataModel.Email))
                {
                    /// Create Consumer Vehicle 
                    var consumer = _consumerRepository.GetFirstOrDefault(v => v.Email.ToLower() == vehicleDataModel.Email.ToLower(), v => v.Vehicle);
                    if (consumer != null)
                    {
                        diagnosticReport.ConsumerId = consumer.Id;
                        var vh = consumer.Vehicle.FirstOrDefault(v => v.Vin.Equals(diagnosticReport.Vin, StringComparison.OrdinalIgnoreCase));
                        if (vh == null)
                        {
                            vh = new Vehicle
                            {
                                Id = Guid.NewGuid(),
                                Vin = report.Vin,
                                Year = report.Year,
                                Make = report.Make,
                                Model = report.Model,
                                Transmission = report.Transmission,
                                TrimLevel = report.TrimLevel,
                                Aaia = report.Aaia,
                                EngineType = report.EngineType,
                                Manufacture = report.Manufacture,
                                Mileage = report.Mileage,
                                IsActive = true,
                                CreatedDate = DateTime.UtcNow,

                            };
                            vh.ImageFileUrl = vehicle.ModelImageFileUrl;
                            vh.ConsumerId = consumer.Id;
                            consumer.Vehicle.Add(vh);
                        }
                        else
                        {
                            vh.IsActive = true;
                            vh.ImageFileUrl = vehicle.ModelImageFileUrl;
                            if (vh.Mileage < report.Mileage)
                            {
                                vh.Mileage = report.Mileage;
                            }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.MilDTC) && diagnosticReport.ReportDtc != null && diagnosticReport.ReportDtc.Any())
                {
                    var monitorFile = diagnosticReport.ReportDtc.FirstOrDefault(e => request.MilDTC.Equals(e.Code, StringComparison.OrdinalIgnoreCase) && e.ReportDtcDefinition != null && e.ReportDtcDefinition.Any(v => !string.IsNullOrEmpty(v.MonitorFile)));
                    if (monitorFile != null)
                    {
                        var definition = monitorFile.ReportDtcDefinition.FirstOrDefault(v => !string.IsNullOrEmpty(v.MonitorFile));
                        if (definition != null)
                        {
                            diagnosticReport.MonitorFile = definition.MonitorFile;
                        }
                    }
                }

                if (vehicleDataModel.Modules != null && vehicleDataModel.Modules.Any())
                {
                    foreach (var module in vehicleDataModel.Modules)
                    {
                        var item = new Data.EntityFramework.ReportModule
                        {
                            Id = Guid.NewGuid(),
                            ModuleName = module.ModuleName,
                            System = module.System,
                            SubSystem = module.SubSystem,
                            InnovaGroup = module.InnovaGroup
                        };

                        if (module.Dtcs != null && module.Dtcs.Any())
                        {
                            foreach (var moduleDtc in module.Dtcs)
                            {
                                var dtc = new ReportModuleDtc
                                {
                                    Id = Guid.NewGuid(),
                                    Code = moduleDtc.Code,
                                    Def = moduleDtc.Def,
                                    Type = moduleDtc.Type
                                };
                                item.ReportModuleDtc.Add(dtc);
                            }
                        }
                        diagnosticReport.ReportModule.Add(item);
                    }
                }

                diagnosticReport.ReportNumber = _diagnosticReportRepository.GetReportNumberByShopId(request.KioskId);
                diagnosticReport = _diagnosticReportRepository.Create(diagnosticReport);

                if (!string.IsNullOrEmpty(vehicleDataModel.Email))
                {
                    var reportUrl = UrlHelper.Merge(CommonConfiguration.ShellConsumerRootUrl,string.Format("{0}?reportId={1}&email={2}", "Home/Consumer", diagnosticReport.Id, vehicleDataModel.Email));

                    var emailModel = diagnosticReport.ToEmailModel(reportUrl);

                    var sendMail = SendMandrillEmailHelper.SendConsumerReportEmail(emailModel);

                    if (!string.IsNullOrEmpty(sendMail))
                    {
                        _logger.Error(string.Format("Send email to customer error {0}", sendMail));
                    }
                }

                if (!string.IsNullOrEmpty(vehicleDataModel.Phone))
                {
                    var sms = string.Format("View your Pro Scan report for {0} - {1}",
                       string.Format("{0} {1} {2}", diagnosticReport.Year, diagnosticReport.Make, diagnosticReport.Model), CommonConfiguration.ShellConsumerRootUrl);
                    var sendText = TwilioSMS.Send(vehicleDataModel.Phone, sms);
                    if (!string.IsNullOrEmpty(sendText))
                    {
                        _logger.Error(string.Format("Send text to customer error {0}", sendText));
                    }
                }

                return new ServiceResult<ReportModel>
                {
                    ResultCode = ResultCode.Ok,
                    Message = "Create Diagnostic Report Succeed.",
                    Data = diagnosticReport.ToApiModelDto(request.TimeZone)
                };

            }
            catch (Exception ex)
            {
                _logger.Fatal(string.Format("[CreateReport] Create a report fail : {0}", LogUtility.GetDetailsErrorMessage(ex)));
                return new ServiceResult<ReportModel>
                {
                    ResultCode = ResultCode.E003001,
                    Message = "Create Diagnostic Report fail.",
                };
            }
        }

        public ServiceResult<ReportModel> CreateInnovaDiagnosticReport(CreateInnovaReportRequest request)
        {
            try
            {
                var vehicleDataModel = request.VehicleInnovaDataModel;
                var vehicle = request.Vehicle;
                request.MilDTC = !string.IsNullOrWhiteSpace(request.MilDTC) ? request.MilDTC : "";

                var reportRequest = CreateReportRequest(vehicle, vehicleDataModel.Mileage, request.RawString);
                var errorMessage = string.Empty;
                var report = _metafuseAdapter.CreateTheDiagnosticReport(reportRequest, out errorMessage);

                if (report == null)
                {
                    _logger.Fatal(string.Format("[CreateReport] Parse Raw String fail : Monitor: {0} - FF: {1} - ECMDTCs: {2} - TCMDTCs: {3} - ex: {4}", vehicleDataModel.MonitorStatusEcmRaw + ";" + vehicleDataModel.MonitorStatusTcmRaw, vehicleDataModel.FreezeFrameEcmRaw + ";" + vehicleDataModel.FreezeFrameTcmRaw, vehicleDataModel.DtcsEcmRaw, vehicleDataModel.DtcsTcmRaw, errorMessage));
                    return new ServiceResult<ReportModel>
                    {
                        ResultCode = ResultCode.E003004,
                        Message = errorMessage,
                    };
                }

                var diagnosticReport = new Report(report);

                //Get 5 Year Cost To Own
                var vehicleRequest = CreateVehicleBaseRequest(vehicle, vehicleDataModel.Mileage);
                var info = _metafuseAdapter.Get5YearCostToOwnInfo(vehicleRequest);
                if (info != null)
                {
                    diagnosticReport.DepreciationCost = info.DepreciationCost;
                    diagnosticReport.FuelCost = info.FuelCost;
                    diagnosticReport.RepairCost = info.RepairCost;
                    diagnosticReport.InsuranceCost = info.InsuranceCost;
                    diagnosticReport.MaintenanceCost = info.MaintenanceCost;
                    diagnosticReport.TotalCostToOwn = info.TotalCostToOwn;
                }

                diagnosticReport.UserId = request.UserId;
                diagnosticReport.KioskId = request.KioskId;

                diagnosticReport.WorkOrderNumber = vehicleDataModel.WorkOrderNumber;
                diagnosticReport.Imei = vehicleDataModel.Imei;
                diagnosticReport.Mileage = vehicleDataModel.Mileage;
                diagnosticReport.Vin = vehicleDataModel.Vin;
                diagnosticReport.ToolMilStatus = request.ToolMilStatus;

                diagnosticReport.OriginalTirePressure = vehicleDataModel.OriginalTirePressure;
                diagnosticReport.Lftire = vehicleDataModel.LFTire;
                diagnosticReport.Rftire = vehicleDataModel.RFTire;
                diagnosticReport.Lrtire = vehicleDataModel.LRTire;
                diagnosticReport.Rrtire = vehicleDataModel.RRTire;
                diagnosticReport.TirePressureUnit = vehicleDataModel.TirePressureUnit;

                diagnosticReport.OriginalBatteryVoltage = vehicleDataModel.OriginalBatteryVoltage;
                diagnosticReport.CurrentBatteryVoltage = vehicleDataModel.CurrentBatteryVoltage;

                diagnosticReport.OilLevel = vehicleDataModel.OilLevel;
                diagnosticReport.BrakePadLife = vehicleDataModel.BrakePadLife;

                diagnosticReport.MonitorStatusRaw = vehicleDataModel.MonitorStatusEcmRaw + ";" + vehicleDataModel.MonitorStatusTcmRaw;
                diagnosticReport.FreezeFrameRaw = vehicleDataModel.FreezeFrameEcmRaw + ";" + vehicleDataModel.FreezeFrameTcmRaw;
                diagnosticReport.EcmdtcsRaw = vehicleDataModel.DtcsEcmRaw;
                diagnosticReport.TcmdtcsRaw = vehicleDataModel.DtcsTcmRaw;
                diagnosticReport.RawString = vehicleDataModel.VinProfileRaw + ";" + request.RawString;

                diagnosticReport.IsVinMismatch = vehicleDataModel.IsVinMismatch;
                diagnosticReport.ToolFirmware = vehicleDataModel.ToolFirmware;
                diagnosticReport.Status = vehicleDataModel.Status;
                diagnosticReport.BatteryStatus = vehicleDataModel.BatteryStatus;

                diagnosticReport.FirstName = vehicleDataModel.FirstName;
                diagnosticReport.LastName = vehicleDataModel.LastName;
                diagnosticReport.Email = vehicleDataModel.Email;
                diagnosticReport.Phone = vehicleDataModel.Phone;

                diagnosticReport.VehicleImageFileUrl = vehicle.ModelImageFileUrl;

                if (!string.IsNullOrEmpty(vehicleDataModel.Email))
                {
                    /// Create Consumer Vehicle 
                    var consumer = _consumerRepository.GetFirstOrDefault(v => v.Email.ToLower() == vehicleDataModel.Email.ToLower(), v => v.Vehicle);
                    if (consumer != null)
                    {
                        diagnosticReport.ConsumerId = consumer.Id;
                        var vh = consumer.Vehicle.FirstOrDefault(v => v.Vin.Equals(diagnosticReport.Vin, StringComparison.OrdinalIgnoreCase));
                        if (vh == null)
                        {
                            vh = new Vehicle
                            {
                                Id = Guid.NewGuid(),
                                Vin = report.Vin,
                                Year = report.Year,
                                Make = report.Make,
                                Model = report.Model,
                                Transmission = report.Transmission,
                                TrimLevel = report.TrimLevel,
                                Aaia = report.Aaia,
                                EngineType = report.EngineType,
                                Manufacture = report.Manufacture,
                                Mileage = report.Mileage,
                                IsActive = true,
                                CreatedDate = DateTime.UtcNow,

                            };
                            vh.ImageFileUrl = vehicle.ModelImageFileUrl;
                            vh.ConsumerId = consumer.Id;
                            consumer.Vehicle.Add(vh);
                        }
                        else
                        {
                            vh.IsActive = true;
                            vh.ImageFileUrl = vehicle.ModelImageFileUrl;
                            if (vh.Mileage < report.Mileage)
                            {
                                vh.Mileage = report.Mileage;
                            }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.MilDTC) && diagnosticReport.ReportDtc != null && diagnosticReport.ReportDtc.Any())
                {
                    var monitorFile = diagnosticReport.ReportDtc.FirstOrDefault(e => request.MilDTC.Equals(e.Code, StringComparison.OrdinalIgnoreCase) && e.ReportDtcDefinition != null && e.ReportDtcDefinition.Any(v => !string.IsNullOrEmpty(v.MonitorFile)));
                    if (monitorFile != null)
                    {
                        var definition = monitorFile.ReportDtcDefinition.FirstOrDefault(v => !string.IsNullOrEmpty(v.MonitorFile));
                        if (definition != null)
                        {
                            diagnosticReport.MonitorFile = definition.MonitorFile;
                        }
                    }
                }

                if (vehicleDataModel.Modules != null && vehicleDataModel.Modules.Any())
                {
                    foreach (var module in vehicleDataModel.Modules)
                    {
                        var item = new Data.EntityFramework.ReportModule
                        {
                            Id = Guid.NewGuid(),
                            ModuleName = module.ModuleName,
                            System = module.System,
                            SubSystem = module.SubSystem,
                            InnovaGroup = module.InnovaGroup
                        };

                        if (module.Dtcs != null && module.Dtcs.Any())
                        {
                            foreach (var moduleDtc in module.Dtcs)
                            {
                                var dtc = new ReportModuleDtc
                                {
                                    Id = Guid.NewGuid(),
                                    Code = moduleDtc.Code,
                                    Def = moduleDtc.Def,
                                    Type = moduleDtc.Type
                                };
                                item.ReportModuleDtc.Add(dtc);
                            }
                        }
                        diagnosticReport.ReportModule.Add(item);
                    }
                }

                diagnosticReport.ReportNumber = _diagnosticReportRepository.GetReportNumberByShopId(request.KioskId);
                diagnosticReport = _diagnosticReportRepository.Create(diagnosticReport);

                if (!string.IsNullOrEmpty(vehicleDataModel.Email))
                {
                    var reportUrl = UrlHelper.Merge(CommonConfiguration.ShellConsumerRootUrl, string.Format("{0}?reportId={1}&email={2}", "Home/Consumer", diagnosticReport.Id, vehicleDataModel.Email));

                    var emailModel = diagnosticReport.ToEmailModel(reportUrl);

                    var sendMail = SendMandrillEmailHelper.SendConsumerReportEmail(emailModel);

                    if (!string.IsNullOrEmpty(sendMail))
                    {
                        _logger.Error(string.Format("Send email to customer error {0}", sendMail));
                    }
                }

                if (!string.IsNullOrEmpty(vehicleDataModel.Phone))
                {
                    var sms = string.Format("View your Pro Scan report for {0} - {1}",
                       string.Format("{0} {1} {2}", diagnosticReport.Year, diagnosticReport.Make, diagnosticReport.Model), CommonConfiguration.ShellConsumerRootUrl);
                    var sendText = TwilioSMS.Send(vehicleDataModel.Phone, sms);
                    if (!string.IsNullOrEmpty(sendText))
                    {
                        _logger.Error(string.Format("Send text to customer error {0}", sendText));
                    }
                }

                return new ServiceResult<ReportModel>
                {
                    ResultCode = ResultCode.Ok,
                    Message = "Create Diagnostic Report Succeed.",
                    Data = diagnosticReport.ToApiModelDto(request.TimeZone)
                };

            }
            catch (Exception ex)
            {
                _logger.Fatal(string.Format("[CreateReport] Create a report fail : {0}", LogUtility.GetDetailsErrorMessage(ex)));
                return new ServiceResult<ReportModel>
                {
                    ResultCode = ResultCode.E003001,
                    Message = "Create Diagnostic Report fail.",
                };
            }
        }

        public ServiceResult<ReportHistoryModel> SearchReports(long kioskId, string timeZone, int pageSize, int pageIndex)
        {
            var results = _diagnosticReportRepository.SearchReports(kioskId, pageSize, pageIndex);

            var result = new ReportHistoryModel
            {
                CurrentPage = results.CurrentPage,
                PageSize = results.PageSize,
                RowCount = results.RowCount,
                PageCount = results.PageCount,
                ReportHistories = results.Data != null ? results.Data.Select(v => v.ToApiHistoryModelDto(timeZone)).ToList() : new List<ReportHistory>()
            };

            return new ServiceResult<ReportHistoryModel>
            {
                ResultCode = ResultCode.Ok,
                Message = "Search Diagnostic Report History succeed.",
                Data = result
            };
        }

        public ServiceResult<ReportModel> GetReportById(long kioskId, string timeZone, Guid id)
        {
            var result = _diagnosticReportRepository.GetFirstOrDefault(d => d.Id == id && d.KioskId == kioskId, d => d.ReportModule.Select(v => v.ReportModuleDtc));

            if (result == null)
            {
                return new ServiceResult<ReportModel>
                {
                    ResultCode = ResultCode.E003000,
                    Message = "Diagnostic Report Not Found.",
                };
            }
            return new ServiceResult<ReportModel>
            {
                ResultCode = ResultCode.Ok,
                Message = "Get Diagnostic Report succeed.",
                Data = result.ToApiModelDto(timeZone)
            };
        }

        public ServiceResult<UserModel> LoginByAuth0(string email)
        {
            var user = _userRepository.GetUser(email);

            if (user == null)
            {
                user = new User()
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    HashPassword = SecurityPassword.HashPassword(RandomStringUtility.Generate(8, false, false)),
                    IsActive = true,
                    TimeZone = "Pacific Standard Time",
                    Auth0Login = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = email,
                    Kiosk = new Kiosk
                    {
                        Country = CountryType.US.ToString(),
                    }
                };
                _userRepository.Create(user);
            }
            if (!user.IsActive)
            {
                return new ServiceResult<UserModel>
                {
                    ResultCode = ResultCode.E002000,
                    Message = "User not found."
                };
            }

            var resultData = user.ToApiDto();

            return new ServiceResult<UserModel>
            {
                ResultCode = ResultCode.Ok,
                Message = "Login succeed.",
                Data = resultData
            };
        }


        private void SendFeedBackEmailToAdmin(List<EmailAddress> toList, FeedBackModel model, bool isTesting = false)
        {
            var kiosk = _kioskRepository.GetFirstOrDefault(k => k.Id == model.KioskId);
            var address = "UnKnown";
            if (kiosk == null)
            {
                _logger.Error(string.Format("[SendFeedBackEmailToAdmin] Kiosk not found for user {0}", model.UserId));
            }
            else
            {
                address = string.Format("{0}", kiosk.Id.ToString().PadLeft(6, '0'));
                if (!string.IsNullOrEmpty(kiosk.Address))
                {
                    address += string.Format(" - {0},", kiosk.Address);
                }
                if (!string.IsNullOrEmpty(kiosk.City))
                {
                    address += string.Format(" {0},", kiosk.City);
                }
                if (!string.IsNullOrEmpty(kiosk.State))
                {
                    address += string.Format(" {0},", kiosk.State);
                }
                if (!string.IsNullOrEmpty(kiosk.PostalCode))
                {
                    address += string.Format(" {0}", kiosk.PostalCode);
                }
            }

            var subject = "";
            var body = "";
            switch (model.Type)
            {
                case (int)FeedBackType.MissingDongle:
                    subject = string.Format("CarMD Alert: Dongle is missing at kiosk {0} with login {1}", address, model.UserEmail);
                    body = string.Format("<p style='text-decoration: underline;'>Kiosk {0}</p><p>User has reported a missing dongle</p>", address);
                    break;
                case (int)FeedBackType.Report:
                    subject = "CarMD Alert: Customer's feedback";
                    body = string.Format("<p style='text-decoration: underline;'>Kiosk {0}<p><p>Feedback: {1}</p>", address, model.Comment);
                    break;
                case (int)FeedBackType.Lockbox:
                    subject = "Lock Box is Open";
                    body = string.Format("<p style='text-decoration: underline;'>Kiosk {0}<p><p>Lock box is open</p>", address);
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(subject))
            {
                var emailMessage = new EmailMessage
                {
                    to = toList,
                    from_email = CommonConfiguration.EmailSenderAddress,
                    subject = isTesting ? "[Testing] " + subject : subject,
                    html = body
                };

                var sendMail = MandrillHelper.SendMessage(emailMessage);

                if (!string.IsNullOrEmpty(sendMail))
                {
                    _logger.Error(string.Format("[SendFeedBackEmailToAdmin] error {0}", sendMail));
                }
            }
        }

        private Adapter.Metafuse.Model.CreateReportRequest CreateReportRequest(VehicleInfo vehicle, int mileage, string rawString)
        {
            var request = new Adapter.Metafuse.Model.CreateReportRequest
            {
                ServiceKey = CommonConfiguration.MetafuseWebServiceKey,
                UserId = new Guid(CommonConfiguration.MetafuseServiceUser),
                FirstName = "CarMD",
                LastName = "Shell",
                EmailAddress = "fleet-shell@carmd.com",
                Region = "CA",
                Vin = vehicle.Vin,
                PhoneNumber = "",
                Mileage = mileage,
                Transmission = vehicle.Transmission,
                RawData = rawString,

                IncludeRecallsForVehicle = true,
                IncludeTsbForVehicleAndMatchingErrorCodes = false,
                IncludeTsbCountForVehicle = true,
                IncludeNextScheduledMaintenance = true,
                IncludeWarrantyInfo = true,
                IncludePredictiveFailureDiagnostic = true,
            };
            return request;
        }

        private VehicleBaseRequest CreateVehicleBaseRequest(VehicleInfo vehicle, int mileage)
        {
            var request = new VehicleBaseRequest
            {
                ServiceKey = CommonConfiguration.MetafuseWebServiceKey,
                Vin = vehicle.Vin,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                EngineType = vehicle.EngineType,
                Transmission = vehicle.Transmission,
                CurrentMileage = mileage
            };
            return request;
        }
    }
}
