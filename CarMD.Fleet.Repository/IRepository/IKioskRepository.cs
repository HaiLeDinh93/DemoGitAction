using CarMD.Fleet.Data.EntityFramework;
using CarMD.Fleet.Data.Request.Kiosk;
using CarMD.Fleet.Data.Response;


namespace CarMD.Fleet.Repository.IRepository
{
    public interface IKioskRepository : IGenericRepository<Kiosk>
    {
        SearchResult<Kiosk> Search(KioskSearchCriteria searchModel);
    }
}
