using CarMD.Fleet.Data.EntityFramework;
using CarMD.Fleet.Repository.EntityFramework;
using CarMD.Fleet.Repository.IRepository;


namespace CarMD.Fleet.Repository.Repository
{
    public class VimeoSettingRepository : GenericRepository<VimeoSetting>, IVimeoSettingRepository
    {
        public VimeoSettingRepository(CarMDShellContext context) : base(context)
        {
        }
    }
}
