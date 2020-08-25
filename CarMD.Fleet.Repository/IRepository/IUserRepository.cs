using CarMD.Fleet.Data.EntityFramework;
using CarMD.Fleet.Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarMD.Fleet.Repository.IRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public User GetUser(string loginString);

        User GetUser(string email, string phone, Guid? excludeUserId = null);
    }
}
