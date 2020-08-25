using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMD.Fleet.Adapter.Metafuse.Model
{
    /// <summary>
    /// This class to contains parameters transfering from outside to web service adapter to invoke the real Metafuse web service.
    /// </summary>
    [Serializable]
    public class CreateReportRequest
    {
        #region Constructor

        public CreateReportRequest()
        {
            IncludeRecallsForVehicle = false;
            IncludeTsbForVehicleAndMatchingErrorCodes = false;
            IncludeTsbCountForVehicle = false;
            IncludeNextScheduledMaintenance = false;
            IncludeWarrantyInfo = false;

            PwrFixNotFoundFixPromisedByDateTimeUtcString = string.Empty;
            Obd1FixNotFoundFixPromisedByDateTimeUtcString = string.Empty;
            AbsFixNotFoundFixPromisedByDateTimeUtcString = string.Empty;
            SrsFixNotFoundFixPromisedByDateTimeUtcString = string.Empty;
        }

        #endregion

        #region Properties

        public string ServiceKey { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Region { get; set; }
        public string Vin { get; set; }
        public int Mileage { get; set; }
        public string Transmission { get; set; }
        public string RawData { get; set; }
        public bool IncludeRecallsForVehicle { get; set; }
        public bool IncludePredictiveFailureDiagnostic { get; set; }
        public bool IncludeTsbForVehicleAndMatchingErrorCodes { get; set; }
        public bool IncludeTsbCountForVehicle { get; set; }
        public bool IncludeNextScheduledMaintenance { get; set; }
        public bool IncludeWarrantyInfo { get; set; }
        
        public string PwrFixNotFoundFixPromisedByDateTimeUtcString { get; set; }
        public string Obd1FixNotFoundFixPromisedByDateTimeUtcString { get; set; }
        public string AbsFixNotFoundFixPromisedByDateTimeUtcString { get; set; }
        public string SrsFixNotFoundFixPromisedByDateTimeUtcString { get; set; }

        #endregion
    }
}
