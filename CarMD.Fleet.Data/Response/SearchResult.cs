using System.Collections.Generic;

namespace CarMD.Fleet.Data.Response
{
    public class SearchResult<T> : ServiceResult<T>
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public new IEnumerable<T> Data { get; set; }

        public SearchResult()
        {
            Data = new List<T>();
        }

        public SearchResult(int totalCount, IEnumerable<T> data)
        {
            RowCount = totalCount;
            Data = data;
        }

        public SearchResult(int currentPage, int totalCount, int totalPage, int pageSize, IEnumerable<T> data)
            : this(totalCount, data)
        {
            CurrentPage = currentPage;
            PageCount = totalPage;
            PageSize = pageSize;
        }
    }
}
