using CarMD.Fleet.Data.Response;

namespace CarMD.Fleet.Data.Dto.Metafuse
{
    public class TSBCategory
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int TSBCount { get; set; }

        public SearchResult<TSB> TSBs { get; set; }
    }
}
