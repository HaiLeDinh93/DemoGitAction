using MetafuseReference;
using CarMD.Fleet.Data.EntityFramework;
using System;
using System.Collections.Generic;
using CarMD.Fleet.Adapter.Metafuse.Model;
using CarMD.Fleet.Data.Dto;
using System.Linq;
using CarMD.Fleet.Data.Dto.Metafuse;
using CarMD.Fleet.Data.Response;
using System.Net;
using VehicleInfo = CarMD.Fleet.Data.Dto.VehicleInfo;

namespace CarMD.Fleet.Adapter.Metafuse
{
    internal class MetafuseAdapter : IMetafuseAdapter
    {
        private readonly IWebServiceInvoker _invoker;

        public MetafuseAdapter(IWebServiceInvoker invoker)
        {          
            _invoker = invoker;
        }

        public Data.Dto.Metafuse.DiagnosticReport CreateTheDiagnosticReport(CreateReportRequest request, out string errorMessage)
        {
            string transmission = CorrectTransmission(request.Transmission);

            var diagReportInfo = _invoker.InvokeWebService<DiagReportInfo>(
                                                    "CreateDiagnosticReport",
                                                     MetafuseServiceInvoker.ServiceKey(request.ServiceKey),
                                                    request.UserId.ToString(),
                                                    request.FirstName ?? string.Empty,
                                                    request.LastName ?? string.Empty,
                                                    request.EmailAddress ?? string.Empty,
                                                    request.PhoneNumber ?? string.Empty,
                                                    string.IsNullOrWhiteSpace(request.Region) ? "CA" : request.Region,
                                                    request.Vin,
                                                    request.Mileage,
                                                    transmission,
                                                    request.IncludeRecallsForVehicle.ToString(),
                                                    request.IncludeTsbForVehicleAndMatchingErrorCodes.ToString(),
                                                    request.IncludeTsbCountForVehicle.ToString(),
                                                    request.IncludeNextScheduledMaintenance.ToString(),
                                                    request.IncludeWarrantyInfo.ToString(),
                                                    1,
                                                    0,
                                                    request.RawData,
                                                    request.PwrFixNotFoundFixPromisedByDateTimeUtcString,
                                                    request.AbsFixNotFoundFixPromisedByDateTimeUtcString,
                                                    request.AbsFixNotFoundFixPromisedByDateTimeUtcString,
                                                    request.AbsFixNotFoundFixPromisedByDateTimeUtcString);

            errorMessage = "";
            if (diagReportInfo == null)
            {
                errorMessage = "Diagnostic Report result from Metafuse is null.";
                return null;
            }
            if (HasError(diagReportInfo.WebServiceSessionStatus, out errorMessage))
            {
                return null;
            }

            //map data to diagnotic report result
            var result = MetafuseMapper.MapDiagnosticReport(diagReportInfo);

            result.RawUploadString = request.RawData;

            #region Map PD
            if (request.IncludePredictiveFailureDiagnostic)
            {
                var predictiveFailures = GetPredictiveDiagnostic(new PredictiveDiagnosticRequest
                {
                    ServiceKey = request.ServiceKey,
                    Vin = request.Vin,
                    CurrentMileage = request.Mileage
                });
                result.PredictiveDiagnostics = predictiveFailures;
            }
            #endregion Map PD

            return result;
        }

        public IList<PredictiveDiagnostic> GetPredictiveDiagnostic(PredictiveDiagnosticRequest request)
        {
            var vehicle = new VehicleRequest()
            {
                WebServiceKey = MetafuseServiceInvoker.ServiceKey(request.ServiceKey),
                VIN = request.Vin,
                CurrentMileage = request.CurrentMileage
            };
            var result = _invoker.InvokeWebService<PredictiveDiagnosticInfo>("GetPredictiveDiagnosticInfo", vehicle);
            if (result != null)
            {
                var items = MetafuseMapper.MapPredictiveDiagnostic(result);
                return items;
            }
            return null;
        }

        public VehicleInfo DecodeVin(string serviceKey, string vin)
        {
            try
            {
                var result = _invoker.InvokeWebService<MetafuseReference.VehicleInfo>("GetVehicleInfo",
                                                                    MetafuseServiceInvoker.ServiceKey(serviceKey), vin);
                string errorMessage;
                if (result == null || HasError(result.ValidationFailures, out errorMessage))
                {
                    return null;
                }
                return MetafuseMapper.MapVehicle(result);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null)
                {
                    var message = ex.InnerException.InnerException.Message;
                    if (!string.IsNullOrWhiteSpace(message) && message.Contains("The following VINs were tried and failed"))
                    {
                        return null;
                    }
                }
                throw ex;
            }
        }

        public DLCLocation GetDLCLocation(string serviceKey, string make, int year, string model)
        {
            var locationInfo = _invoker.InvokeWebService<DLCLocationInfo[]>("GetDLCLocationForVehicleByYearMakeModel", MetafuseServiceInvoker.ServiceKey(serviceKey), make, year.ToString(), model);
            if (locationInfo != null && locationInfo.Length > 0)
            {
                return MetafuseMapper.MapDLCLocation(locationInfo[0]);
            }
            return null;
        }

        public DLCLocation GetDLCLocationByVin(string serviceKey, string vin)
        {
            var vehicle = DecodeVin(serviceKey, vin);

            DLCLocation dlc = null;

            if (vehicle != null)
            {
                dlc = GetDLCLocation(serviceKey, vehicle.Make, vehicle.Year, vehicle.Model);
                if (dlc != null)
                {
                    dlc.EngineType = vehicle.EngineType;
                    dlc.Vin = vin;
                }
            }

            return dlc;
        }

        public IEnumerable<string> GetMakes(string serviceKey)
        {
            var makes = _invoker.InvokeWebService<string[]>("GetDLCLocationAvailableMakes", MetafuseServiceInvoker.ServiceKey(serviceKey));
            return makes;
        }

        public IEnumerable<int> GetDLCYears(string serviceKey, string make)
        {
            var years = _invoker.InvokeWebService<string[]>("GetDLCLocationAvailableYears", MetafuseServiceInvoker.ServiceKey(serviceKey), make);
            var yearsInt = new List<int>();
            years.ToList().ForEach(y => yearsInt.Add(int.Parse(y)));
            return yearsInt;
        }

        public IEnumerable<string> GetMakesByYear(string serviceKey, int[] year, string[] make = null)
        {
            string[] years = year != null && year.Count() > 0 ? year.Select(x => x.ToString()).ToArray() : new string[0];
            var makes = _invoker.InvokeWebService<string[]>("GetAvailableMakesByYearsAndMakes", MetafuseServiceInvoker.ServiceKey(serviceKey), years, make ?? new string[0]);
            return makes;
        }

        public IEnumerable<int> GetYears(string serviceKey, string make, string model, bool isAll = false)
        {
            string manufacturer = "";
            var years = _invoker.InvokeWebService<string[]>("GetAvailableYears", MetafuseServiceInvoker.ServiceKey(serviceKey), manufacturer, make, model);
            var yearsInt = new List<int>();
            if (years != null && years.Any())
            {
                if (isAll)
                {
                    years.ToList().ForEach(y => yearsInt.Add(int.Parse(y)));
                }
                else
                {
                    foreach (var y in years)
                    {
                        int year;
                        if (int.TryParse(y, out year) && year >= 1996)
                            yearsInt.Add(year);
                    }
                }
            }
            return yearsInt;
        }

        public IEnumerable<string> GetDLCModels(string serviceKey, string make, int year)
        {
            var models = _invoker.InvokeWebService<string[]>("GetDLCLocationAvailableModels", MetafuseServiceInvoker.ServiceKey(serviceKey), make, year.ToString());
            return models;
        }

        public IEnumerable<TSBCategory> GetTSBCountByCategory(string serviceKey, string vin)
        {
            var tsbCategoryInfo = _invoker.InvokeWebService<TSBCategoryInfo[]>("GetTSBCountByVehicleByCategory", MetafuseServiceInvoker.ServiceKey(serviceKey), vin);
            if (tsbCategoryInfo == null || tsbCategoryInfo.Length == 0)
            {
                return new List<TSBCategory>();
            }
            return MetafuseMapper.MapTSBCategories(tsbCategoryInfo);
        }

        public SearchResult<TSB> SearchTSBs(TsbSearchCriteria criteria)
        {
            var externalTSBs = _invoker.InvokeWebService<TSBInfoPageSpanned>("GetTSBsForVehicleAndErrorCodeListPageSpanned", MetafuseServiceInvoker.ServiceKey(criteria.WebServiceKey),
                                                     criteria.CurrentPage, criteria.PageSize,
                                                     criteria.Vin, criteria.Category, criteria.CommaSeparatedErrorCodes);
            if ((externalTSBs == null) || (externalTSBs.TSBInfos != null && externalTSBs.TSBInfos.Length == 0))
            {
                return new SearchResult<TSB>(criteria.CurrentPage, 0, 0, criteria.PageSize, new List<TSB>());
            }

            return new SearchResult<TSB>(criteria.CurrentPage,
                            externalTSBs.PageSpanInfo.RecordCount,
                            externalTSBs.PageSpanInfo.PageCount,
                            criteria.PageSize,
                            MetafuseMapper.MapTSBs(externalTSBs.TSBInfos));
        }

        public FiveYearCostToOwnInfo Get5YearCostToOwnInfo(VehicleBaseRequest request)
        {
            var vehicleRequest = new VehicleRequest
            {
                WebServiceKey = MetafuseServiceInvoker.ServiceKey(request.ServiceKey),
                PageSpanConfig = new PageSpanConfig { PageSize = 200, CurrentPage = 1 },
                VIN = request.Vin,
                Make = request.Make ?? string.Empty,
                Model = request.Model ?? string.Empty,
                Year = request.Year,
                EngineType = request.EngineType ?? string.Empty,
                Transmission = request.Transmission ?? string.Empty,
                CurrentMileage = request.CurrentMileage,
            };

            var result = _invoker.InvokeWebService<VehicleCostToOwnInfo>("Get5YearCostToOwnInfo", vehicleRequest);
            if (result == null)
            {
                return null;
            }

            var info = MetafuseMapper.Map5YearCostToOwnInfo(result);
            return info;
        }

        #region Private method

        /// <summary>
        /// check if there is any error
        /// </summary>
        /// <param name="validationFailure"></param>
        /// <param name="message"></param>
        /// <returns>true: if there is any error</returns>
        private static bool HasError(ValidationFailure[] validationFailure, out string message)
        {
            message = string.Empty;
            if (validationFailure != null && validationFailure.Length > 0)
            {
                message = validationFailure[0].Description;
                return true;
            }
            return false;
        }

        /// <summary>
        /// check if there is any error
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <returns>true if there is any error</returns>
        private static bool HasError(WebServiceSessionStatus result, out string message)
        {
            message = string.Empty;
            if (result != null && result.ValidationFailures != null
                && result.ValidationFailures.Length != 0)
            {
                message = result.ValidationFailures[0].Description;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the transmission of vehicle.
        /// </summary>
        /// <param name="transmission">transmission</param>
        /// <returns></returns>
        private static string CorrectTransmission(string transmission)
        {
            string transmissionType = "standard";

            if (!string.IsNullOrEmpty(transmission) && transmission.ToLower() != "manual")
            {
                transmissionType = transmission.ToLower();
            }

            return transmissionType;
        }

        #endregion

    }
}
