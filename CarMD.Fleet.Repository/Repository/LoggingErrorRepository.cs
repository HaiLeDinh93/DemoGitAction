using CarMD.Fleet.Data.Response;
using CarMD.Fleet.Repository.EntityFramework;
using CarMD.Fleet.Repository.IRepository;
using CarMD.Fleet.Data.Request;
using CarMD.Fleet.Core.Utility.Extensions;
using CarMD.Fleet.Repository.Helpers;
using CarMD.Fleet.Data.EntityFramework;

namespace CarMD.Fleet.Repository.Repository
{
    public class LoggingErrorRepository : GenericRepository<LoggingError>, ILoggingErrorRepository
    {
        public LoggingErrorRepository(CarMDShellContext context) : base(context)
        {
        }

        public SearchResult<LoggingError> Search(SearchCriteria searchCriteria)
        {
            var query = Query();

            var sortColumn = string.IsNullOrEmpty(searchCriteria.SortColumn) ? "CreatedDate" : searchCriteria.SortColumn;
            var sortExpression = string.Format("{0} {1}", sortColumn,
                                               searchCriteria.SortDirection == SortDirection.Nosort
                                                   ? SortDirection.Desc
                                                   : searchCriteria.SortDirection);

            query = query.OrderBy(sortExpression);

            var pageData = PagingHelper.GetPage(query, searchCriteria);

            return pageData;
        }
    }
}
