using CarMD.Fleet.Data.Response;

namespace CarMD.Fleet.Service.IService
{
    public interface IUserService
    {
        ServiceResult<bool> ResetPassword(string email);
    }
}
