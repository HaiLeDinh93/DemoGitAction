using CarMD.Fleet.Data.EntityFramework;
using CarMD.Fleet.Data.Request;
using CarMD.Fleet.Data.Response;

namespace CarMD.Fleet.Repository.IRepository
{
    public interface ILoggingErrorRepository : IGenericRepository<LoggingError>
    {
        SearchResult<LoggingError> Search(SearchCriteria searchCriteria);
    }
}
