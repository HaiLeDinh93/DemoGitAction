using System.Linq;
using System.Collections.Generic;
using CarMD.Fleet.Repository.EntityFramework;
using CarMD.Fleet.Repository.IRepository;
using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Core.Utility.Extensions;
using CarMD.Fleet.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace CarMD.Fleet.Repository.Repository
{
    public class FeedBackRepository : GenericRepository<FeedBack>, IFeedBackRepository
    {
        public FeedBackRepository(CarMDShellContext context) : base(context)
        {

        }

        public List<FeedBack> GetDongleNotify(long kioskId)
        {
            return Query(v => v.KioskId == kioskId && v.Type == (int)FeedBackType.MissingDongle && !v.IsSent)
            .DistinctBy(v => v.CustomerEmail).ToList();
        }

        [System.Obsolete]
        public void UpdateDongleNotify(long kioskId)
        {
            Context.Database.ExecuteSqlCommand("NotifyHavingDongleSuccess @p0", kioskId);           
        }
    }
}
