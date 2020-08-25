using CarMD.Fleet.Core.Utility;
using CarMD.Fleet.Data.EntityFramework;
using CarMD.Fleet.Repository.EntityFramework;
using CarMD.Fleet.Repository.IRepository;
using System;

namespace CarMD.Fleet.Repository.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(CarMDShellContext context) : base(context)
        {

        }
        public User GetUser(string loginString)
        {
            loginString = string.IsNullOrEmpty(loginString) ? "" : loginString.ToLower();
            var phone = PhoneNumberFormatter.GetValue(loginString);

            return GetFirstOrDefault(u => u.Email.ToLower() == loginString ||
                                       u.MobilePhone.ToLower()== phone,
                                       u => u.Kiosk);
        }

        public User GetUser(string email, string phone, Guid? excludeUserId = null)
        {
            email = string.IsNullOrEmpty(email) ? "" : email.ToLower();
            phone = PhoneNumberFormatter.GetValue(phone);

            if (excludeUserId.HasValue)
            {
                return GetFirstOrDefault(u => u.Id != excludeUserId.Value && (u.Email.ToLower() == email || u.MobilePhone.ToLower() == phone));
            }

            return GetFirstOrDefault(u => u.Email.ToLower() == email || u.MobilePhone.ToLower() == phone);
        }
    }
}
