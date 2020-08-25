using CarMD.Fleet.Adapter.Metafuse.Model;
using CarMD.Fleet.Data.Dto;
using CarMD.Fleet.Data.Dto.Metafuse;
using CarMD.Fleet.Data.Response;
using System.Collections.Generic;

namespace CarMD.Fleet.Adapter.Metafuse
{
    public interface IMetafuseAdapter
    {
        DiagnosticReport CreateTheDiagnosticReport(CreateReportRequest request, out string errorMessage);

        VehicleInfo DecodeVin(string serviceKey, string vin);

        DLCLocation GetDLCLocation(string serviceKey, string make, int year, string model);

        DLCLocation GetDLCLocationByVin(string serviceKey, string vin);

        IEnumerable<string> GetMakes(string serviceKey);

        IEnumerable<int> GetDLCYears(string serviceKey, string make);

        IEnumerable<string> GetDLCModels(string serviceKey, string make, int year);

        IEnumerable<TSBCategory> GetTSBCountByCategory(string serviceKey, string vin);

        SearchResult<TSB> SearchTSBs(TsbSearchCriteria criteria);

        FiveYearCostToOwnInfo Get5YearCostToOwnInfo(VehicleBaseRequest request);

        IEnumerable<string> GetMakesByYear(string serviceKey, int[] year, string[] make = null);

        IEnumerable<int> GetYears(string serviceKey, string make, string model, bool isAll = false);
    }
}
