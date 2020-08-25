using CarMD.Fleet.Core.Utility;
using CarMD.Fleet.Data.EntityFramework;
using CarMD.Fleet.Repository.EntityFramework;
using CarMD.Fleet.Repository.IRepository;
using System;

namespace CarMD.Fleet.Repository.Repository
{

    public class ConsumerRepository : GenericRepository<Consumer>, IConsumerRepository
    {
        public ConsumerRepository(CarMDShellContext context) : base(context)
        {

        }
    }
}
