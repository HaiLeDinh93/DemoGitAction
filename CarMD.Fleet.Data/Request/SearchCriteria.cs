using System;
using System.ComponentModel;

namespace CarMD.Fleet.Data.Request
{
    #region Search Criteria

    public enum SortDirection
    {
        [Description("Sort by ascending")]
        Asc,

        [Description("Sort by ascending")]
        Desc,

        [Description("No sort")]
        Nosort
    }

    [Serializable]
    public abstract class BaseSearchCriteria
    {
        private int _currentPage = 1;
        private int _pageSize = int.MaxValue;

        public int CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        public int SkipCount
        {
            get { return (CurrentPage - 1) * PageSize; }
        }

        public string Query { get; set; }

        public string SortColumn { get; set; }

        public SortDirection SortDirection { get; set; }
    }

    /// <summary>
    /// Search criteria.
    /// </summary>
    [Serializable]
    public class SearchCriteria : BaseSearchCriteria
    {
        public SearchCriteria()
        {
        }

        public SearchCriteria(int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
    }

    /// <summary>
    /// Search criteria.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class SearchCriteria<T> : BaseSearchCriteria
    {
        public SearchCriteria()
        {
        }

        public SearchCriteria(int currentPage, int pageSize, T data)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            Data = data;
        }

        public T Data { get; set; }
    }

    #endregion
}
