using CarMD.Fleet.Service.IService;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CarMD.Fleet.Data.Response;
using CarMD.Fleet.Core.Utility;
using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Data.Dto;
using CarMD.Fleet.Adapter.Metafuse;
using CarMD.Fleet.Common.Configuration;
using CarMD.Fleet.Core.Utility.Extensions;

namespace CarMD.Fleet.Service.Service
{
    public class VehicleService : IVehicleService
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IMetafuseAdapter _metafuseAdapter;

        public VehicleService(IMetafuseAdapter metafuseAdapter)
        {
            _metafuseAdapter = metafuseAdapter;
        }

        public ServiceResult<VehicleInfo> DecodeVin(string vin)
        {
            try
            {

                var v = _metafuseAdapter.DecodeVin(CommonConfiguration.MetafuseWebServiceKey, vin);
                if (v != null)
                {
                    return new ServiceResult<VehicleInfo>
                    {
                        ResultCode = ResultCode.Ok,
                        Data = v
                    };
                }
                else
                {
                    return new ServiceResult<VehicleInfo>
                    {
                        ResultCode = ResultCode.E005006,
                        Message = "Invalid VIN."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.FatalFormat("[Decode Vin] exception: \r\n Vin :{0} \r\n Exception: {1}", vin, LogUtility.GetDetailsErrorMessage(ex));

                return new ServiceResult<VehicleInfo>
                {
                    ResultCode = ResultCode.E005001,
                    Message = "There is an error. Please contact to admin for support."
                };
            }

        }

        public ServiceResult<IEnumerable<string>> Makes(int year)
        {
            try
            {
                IEnumerable<string> makes;
                if (year > 0)
                {
                    makes = _metafuseAdapter.GetMakesByYear(CommonConfiguration.MetafuseWebServiceKey, new int[] { year });
                }
                else
                {
                    makes = _metafuseAdapter.GetMakes(CommonConfiguration.MetafuseWebServiceKey);
                }
                return new ServiceResult<IEnumerable<string>>
                {
                    ResultCode = ResultCode.Ok,
                    Message = "Get Makes succeed.",
                    Data = makes
                };
            }
            catch (Exception ex)
            {
                _logger.FatalFormat("[GetMakes] Exception {0}", LogUtility.GetDetailsErrorMessage(ex));
                throw (ex);
            }
        }

        public ServiceResult<IEnumerable<int>> Years(string make)
        {
            try
            {
                IEnumerable<int> years;
                if (!string.IsNullOrEmpty(make))
                {
                    years = _metafuseAdapter.GetDLCYears(CommonConfiguration.MetafuseWebServiceKey, make);
                }
                else
                {
                    years = _metafuseAdapter.GetYears(CommonConfiguration.MetafuseWebServiceKey, "", "");
                }

                return new ServiceResult<IEnumerable<int>>
                {
                    ResultCode = ResultCode.Ok,
                    Message = "Get Years succeed.",
                    Data = years
                };
            }
            catch (Exception ex)
            {
                _logger.FatalFormat("[GetYears] Exception {0}", LogUtility.GetDetailsErrorMessage(ex));
                throw (ex);
            }
        }


        public ServiceResult<IEnumerable<string>> Models(string make, int year)
        {
            try
            {
                var years = _metafuseAdapter.GetDLCModels(CommonConfiguration.MetafuseWebServiceKey, make, year);
                return new ServiceResult<IEnumerable<string>>
                {
                    ResultCode = ResultCode.Ok,
                    Message = "Get Models succeed.",
                    Data = years
                };
            }
            catch (Exception ex)
            {
                _logger.FatalFormat("[GetModels] Exception {0}", LogUtility.GetDetailsErrorMessage(ex));
                throw (ex);
            }
        }

        public ServiceResult<DLCLocation> DLC(string make, int year, string model, string timeZone = "")
        {
            try
            {
                var createdDateTimeUtc = DateTime.UtcNow;

                var dlc = _metafuseAdapter.GetDLCLocation(CommonConfiguration.MetafuseWebServiceKey, make, year, model);
                if (dlc != null)
                {
                    if (!string.IsNullOrWhiteSpace(timeZone))
                    {
                        var report = GetReportHistory(make, year.ToString(), model, timeZone, createdDateTimeUtc, "", "");
                        dlc.Report = report;
                    }

                    return new ServiceResult<DLCLocation>
                    {
                        ResultCode = ResultCode.Ok,
                        Message = "Get DLC succeed.",
                        Data = dlc
                    };
                }

                return new ServiceResult<DLCLocation>
                {
                    ResultCode = ResultCode.E005007,
                    Message = "Cannot decode DLC Location."
                };
            }
            catch (Exception ex)
            {
                _logger.FatalFormat("[GetDLC] Exception {0}", LogUtility.GetDetailsErrorMessage(ex));
                throw ex;
            }
        }

        public ServiceResult<DLCLocation> DLCByVin(string vin, string timeZone = "")
        {
            try
            {
                var createdDateTimeUtc = DateTime.UtcNow;

                var dlc = _metafuseAdapter.GetDLCLocationByVin(CommonConfiguration.MetafuseWebServiceKey, vin);
                if (dlc != null)
                {
                    if (!string.IsNullOrWhiteSpace(timeZone))
                    {
                        var report = GetReportHistory(dlc.Make, dlc.Year, dlc.Model, timeZone, createdDateTimeUtc, dlc.Vin, dlc.EngineType);
                        dlc.Report = report;
                    }

                    return new ServiceResult<DLCLocation>
                    {
                        ResultCode = ResultCode.Ok,
                        Message = "Get DLC succeed.",
                        Data = dlc
                    };
                }

                return new ServiceResult<DLCLocation>
                {
                    ResultCode = ResultCode.E005007,
                    Message = "Cannot decode DLC Location."
                };
            }
            catch (Exception ex)
            {
                _logger.FatalFormat("[GetDLC] Exception {0}", LogUtility.GetDetailsErrorMessage(ex));
                throw ex;
            }
        }

        private ReportHistory GetReportHistory(string make, string year, string model, string timeZone, DateTime createdDateTimeUtc, string vin, string engine)
        {
            var target = new ReportHistory
            {
                Id = Guid.NewGuid(),
                WorkOrderNumber = "",
                ReportNumber = 0,
                Vin = vin,
                Mileage = 0,
                CreatedDateTimeUtc = createdDateTimeUtc.ToString("yyyy-MM-ddTHH:mm:ss")
            };

            var engineType = string.IsNullOrWhiteSpace(engine) ? string.Empty : engine.Split(';').First();
            target.YMM = string.Format("{0} {1} {2}", year, make, model);
            target.YMME = string.Format("{0} {1} {2} {3}", make, make, model, engineType);
            target.Engine = engineType;

            var createdDate = createdDateTimeUtc.ConvertToTimeZone(timeZone);
            target.CreatedDate = createdDate.ToString("MM/dd/yy");
            target.CreatedTime = string.Format("{0:t}", createdDate);
            target.CreatedDateTime = createdDate.ToString("yyyy-MM-ddTHH:mm:ss");
            target.TimeAgo = createdDateTimeUtc.GetTimeAgo();

            return target;
        }

    }
}
