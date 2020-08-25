using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Core.Utility.Extensions.LinqKit;
using CarMD.Fleet.Data.EntityFramework;
using CarMD.Fleet.Data.Request.Api;
using CarMD.Fleet.Data.Response;
using CarMD.Fleet.Repository.EntityFramework;
using CarMD.Fleet.Repository.Helpers;
using CarMD.Fleet.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;
using Z.EntityFramework.Plus;

namespace CarMD.Fleet.Repository.Repository
{
    public class DiagnosticReportRepository : GenericRepository<Report>, IDiagnosticReportRepository
    {
        public DiagnosticReportRepository(CarMDShellContext context) : base(context)
        {

        }

        public long GetReportNumberByShopId(long kioskId)
        {
            IQueryable<ViewCurrentReportNumber> query = Context.ViewCurrentReportNumber;
            var view = query.FirstOrDefault(v => v.KioskId == kioskId);
            var reportNumber = view != null ? view.ReportNumber + 1 : 1;
            return reportNumber ?? 1;
        }

        public ServiceResult<bool> EraseModuleDtcs(long kioskId, EraseModuleDtcModel model)
        {
            var report = GetFirstOrDefault(v => v.Id == model.ReportId && v.KioskId == kioskId, v => v.ReportModule.Select(m => m.ReportModuleDtc));
            if (report == null)
            {
                return new ServiceResult<bool>
                {
                    ResultCode = ResultCode.E003000,
                    Message = "Diagnostic Report Not Found.",
                    Data = false
                };
            }
            ICollection<ReportModule> modules = null;
            if (model.ClearAll)
            {
                modules = report.ReportModule;
            }
            else if (model.ModuleId.HasValue)
            {
                modules = report.ReportModule.Where(v => v.Id == model.ModuleId.Value).ToList();
            }

            if (modules == null || !modules.Any())
            {
                return new ServiceResult<bool>
                {
                    ResultCode = ResultCode.E003003,
                    Message = "Report module Not Found.",
                    Data = false
                };
            }

            modules.ForEach(v =>
            {
                Context.ReportModuleDtc.RemoveRange(v.ReportModuleDtc);
                Context.SaveChanges();
            });

            return new ServiceResult<bool>
            {
                ResultCode = ResultCode.Ok,
                Message = "Erase Module Dtc success.",
                Data = true
            };
        }

        public SearchResult<Report> SearchReports(long kioskId, int pageSize, int pageIndex)
        {
            var report = Query(d => d.KioskId == kioskId).OrderByDescending(d => d.CreatedDateTimeUtc);
            var pageData = PagingHelper.GetPage(report, pageSize, pageIndex);
            return pageData;
        }

    }
 
}
