using CarMD.Fleet.Data.EntityFramework;

namespace CarMD.Fleet.Service.IService
{
    public interface IVimeoSettingService
    {
        VimeoSetting Get(int type = 0);
    }
}
