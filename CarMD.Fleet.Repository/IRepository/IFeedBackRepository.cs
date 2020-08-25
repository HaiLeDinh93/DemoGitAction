using CarMD.Fleet.Data.EntityFramework;
using System.Collections.Generic;

namespace CarMD.Fleet.Repository.IRepository
{
    public interface IFeedBackRepository : IGenericRepository<FeedBack>
    {
        List<FeedBack> GetDongleNotify(long kioskId);

        void UpdateDongleNotify(long kioskId);
    }
}
