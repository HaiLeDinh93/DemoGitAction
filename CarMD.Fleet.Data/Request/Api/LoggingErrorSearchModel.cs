using CarMD.Fleet.Data.EntityFramework;
using System.Collections.Generic;

namespace CarMD.Fleet.Data.Request.Api
{
    public class LoggingErrorSearchModel
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public List<LoggingError> Logs { get; set; }
    }
}
