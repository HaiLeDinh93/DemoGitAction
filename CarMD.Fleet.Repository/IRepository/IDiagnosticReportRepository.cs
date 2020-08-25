using CarMD.Fleet.Data.Response;
using CarMD.Fleet.Data.EntityFramework;
using CarMD.Fleet.Data.Request.Api;

namespace CarMD.Fleet.Repository.IRepository
{
    public interface IDiagnosticReportRepository : IGenericRepository<Report>
    {
        long GetReportNumberByShopId(long kioskId);

        ServiceResult<bool> EraseModuleDtcs(long kioskId, EraseModuleDtcModel model);

        SearchResult<Report> SearchReports(long kioskId, int pageSize, int pageIndex);
    }
}
