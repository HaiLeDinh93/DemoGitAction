using CarMD.Fleet.Data.Dto;
using CarMD.Fleet.Data.Response;
using System.Collections.Generic;

namespace CarMD.Fleet.Service.IService
{
    public interface IVehicleService
    {
        ServiceResult<VehicleInfo> DecodeVin(string vin);

        ServiceResult<IEnumerable<string>> Makes(int year);

        ServiceResult<IEnumerable<int>> Years(string make);

        ServiceResult<IEnumerable<string>> Models(string make, int year);

        ServiceResult<DLCLocation> DLC(string make, int year, string model, string timeZone = "");

        ServiceResult<DLCLocation> DLCByVin(string vin, string timeZone = "");
    }
}
