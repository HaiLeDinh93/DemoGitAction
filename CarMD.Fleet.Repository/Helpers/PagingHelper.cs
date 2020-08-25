using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Data.Request;
using CarMD.Fleet.Data.Response;
using System.Linq;

namespace CarMD.Fleet.Repository.Helpers
{
    public class PagingHelper
    {
        public static SearchResult<T> GetPage<T>(IQueryable<T> data, SearchCriteria pagingCriteria)
        {
            var result = new SearchResult<T>
            {
                CurrentPage = pagingCriteria.CurrentPage,
                PageSize = pagingCriteria.PageSize,
                RowCount = data.Count()
            };
            result.PageCount = result.RowCount / result.PageSize + (result.RowCount % result.PageSize > 0 ? 1 : 0);
            result.Data = data.Skip((result.CurrentPage - 1) * result.PageSize).Take(result.PageSize);
            result.ResultCode = ResultCode.Ok;

            return result;
        }

        public static SearchResult<T> GetPage<T>(IQueryable<T> data, int pageSize, int currentPage)
        {
            var result = new SearchResult<T>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                RowCount = data.Count()
            };
            result.PageCount = result.RowCount / result.PageSize + (result.RowCount % result.PageSize > 0 ? 1 : 0);
            result.Data = data.Skip((result.CurrentPage - 1) * result.PageSize).Take(result.PageSize);
            result.ResultCode = ResultCode.Ok;

            return result;
        }

        public static SearchResult<T> GetPage<T, D>(IQueryable<T> data, SearchCriteria<D> pagingCriteria)
        {
            var result = new SearchResult<T>
            {
                CurrentPage = pagingCriteria.CurrentPage,
                PageSize = pagingCriteria.PageSize,
                RowCount = data.Count()
            };
            result.PageCount = result.RowCount / result.PageSize + (result.RowCount % result.PageSize > 0 ? 1 : 0);
            result.Data = data.Skip((result.CurrentPage - 1) * result.PageSize).Take(result.PageSize);
            result.ResultCode = ResultCode.Ok;

            return result;
        }
    }
}
