using CarMD.Fleet.Data.EntityFramework;
using CarMD.Fleet.Data.Request.Kiosk;
using CarMD.Fleet.Data.Response;
using CarMD.Fleet.Repository.EntityFramework;
using CarMD.Fleet.Repository.IRepository;
using CarMD.Fleet.Core.Utility;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CarMD.Fleet.Data.Request;
using CarMD.Fleet.Repository.Helpers;
using CarMD.Fleet.Core.Utility.Extensions;

namespace CarMD.Fleet.Repository.Repository
{
    public class KioskRepository : GenericRepository<Kiosk>, IKioskRepository
    {
        public KioskRepository(CarMDShellContext context) : base(context)
        {
        }

        public SearchResult<Kiosk> Search(KioskSearchCriteria searchModel)
        {
            IQueryable<Kiosk> kiosks = Query().Include(v => v.User);

            if (!string.IsNullOrEmpty(searchModel.FirstName))
            {
                kiosks = kiosks.Where(v => v.User.Any(u => u.FirstName.Contains(searchModel.FirstName)));
            }
            if (!string.IsNullOrEmpty(searchModel.LastName))
            {
                kiosks = kiosks.Where(v => v.User.Any(u => u.LastName.Contains(searchModel.LastName)));
            }
            if (!string.IsNullOrEmpty(searchModel.City))
            {
                kiosks = kiosks.Where(v => v.City.Contains(searchModel.City));
            }
            if (!string.IsNullOrEmpty(searchModel.Country))
            {
                kiosks = kiosks.Where(v => v.Country.Contains(searchModel.Country));
            }
            if (!string.IsNullOrEmpty(searchModel.State))
            {
                kiosks = kiosks.Where(v => v.State.Contains(searchModel.State));
            }
            if (!string.IsNullOrEmpty(searchModel.PostalCode))
            {
                kiosks = kiosks.Where(v => v.PostalCode.Contains(searchModel.PostalCode));
            }
            if (!string.IsNullOrEmpty(searchModel.EmailAddress))
            {
                kiosks = kiosks.Where(v => v.User.Any(u => u.Email.Contains(searchModel.EmailAddress)));
            }
            if (!string.IsNullOrEmpty(searchModel.PhoneNumber))
            {
                var phone = PhoneNumberFormatter.GetValue(searchModel.PhoneNumber);
                kiosks = kiosks.Where(v => v.User.Any(u => u.MobilePhone.Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Contains(phone)));
            }

            var sortColumn = string.IsNullOrEmpty(searchModel.SortColumn) ? "Id" : searchModel.SortColumn;
            var sortExpression = string.Format("{0} {1}", sortColumn,
                                               searchModel.SortDirection == SortDirection.Nosort
                                                   ? SortDirection.Desc
                                                   : searchModel.SortDirection);

            kiosks = kiosks.OrderBy(sortExpression);

            var pageData = PagingHelper.GetPage(kiosks, searchModel);

            return pageData;
        }
    }
}
