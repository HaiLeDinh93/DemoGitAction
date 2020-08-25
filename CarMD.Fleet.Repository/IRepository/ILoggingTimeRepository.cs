using CarMD.Fleet.Data.EntityFramework;
using CarMD.Fleet.Data.Request;
using CarMD.Fleet.Data.Response;
using CarMD.Fleet.Repository.EntityFramework;

namespace CarMD.Fleet.Repository.IRepository
{
    public interface ILoggingTimeRepository : IGenericRepository<LoggingTime>
    {
        SearchResult<LoggingTime> Search(SearchCriteria searchCriteria);
    }
}
