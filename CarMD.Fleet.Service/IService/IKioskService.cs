using CarMD.Fleet.Data.EntityFramework;
using CarMD.Fleet.Data.Request.Api;
using CarMD.Fleet.Data.Request.Kiosk;
using CarMD.Fleet.Data.Response;
using System.Collections.Generic;

namespace CarMD.Fleet.Service.IService
{
    public interface IKioskService
    {
        List<Kiosk> GetAll();

        ServiceResult<bool> Update(UpdateKioskModel model);

        ServiceResult<bool> Ping(long id);

    }
}
