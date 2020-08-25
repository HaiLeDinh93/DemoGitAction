using CarMD.Fleet.Data.Dto;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.Response.Api
{
    public class ReportHistoryModel
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public List<ReportHistory> ReportHistories { get; set; }
    }
}
