using CarMD.Fleet.Repository.IRepository;
using CarMD.Fleet.Service.IService;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CarMD.Fleet.Data.Request.Kiosk;
using CarMD.Fleet.Data.Response;
using CarMD.Fleet.Data.EntityFramework;
using CarMD.Fleet.Core.Utility;
using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Data.Email;
using CarMD.Fleet.Common.Configuration;
using CarMD.Fleet.Service.Mandrill;
using CarMD.Fleet.Common.Helper;
using CarMD.Fleet.Core.Common;
using CarMD.Fleet.Core.Cryptography;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Http;
using CarMD.Fleet.Common.Helpers;
using CarMD.Fleet.Data.Request.Api;

namespace CarMD.Fleet.Service.Service
{
    public class KioskService : IKioskService
    {
        private readonly IKioskRepository _kioskRepository;

        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public KioskService(IKioskRepository kioskRepository)
        {
            _kioskRepository = kioskRepository;
        }

        public ServiceResult<bool> Update(UpdateKioskModel model)
        {
            try
            {
                var kiosk = _kioskRepository.GetFirstOrDefault(v => v.Id == model.Id);
                if (kiosk == null)
                {
                    return new ServiceResult<bool>
                    {
                        ResultCode = Common.Enum.ResultCode.DataInvaild,
                        Message = "Kiosk not found."
                    };
                }
                kiosk.Address = model.Address1;
                kiosk.Address1 = model.Address2;
                kiosk.City = model.City;
                kiosk.Country = model.Country;
                kiosk.PostalCode = model.PostalCode;
                kiosk.State = model.State;
                _kioskRepository.Update(kiosk);
                return new ServiceResult<bool>
                {
                    ResultCode = Common.Enum.ResultCode.Ok,
                    Message = "Update Kiosk Sucess.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.FatalFormat("[Create Kiosk] Create error : {0}", LogUtility.GetDetailsErrorMessage(ex));

                return new ServiceResult<bool>
                {
                    ResultCode = Common.Enum.ResultCode.Error,
                    Message = "This is an error. Please contact admin to support.",
                };
            }

        }

        public ServiceResult<bool> Ping(long id )
        {
            var kiosk = _kioskRepository.GetFirstOrDefault(v => v.Id == id);
            if (kiosk == null)
            {
                return new ServiceResult<bool>
                {
                    Data = false,
                    ResultCode = ResultCode.DataInvaild,
                    Message = "Kiosk id not found"
                };
            }
            kiosk.Status = (int)KioskStatus.Active;

            kiosk.KioskPing.Add(new KioskPing
            {
                Id = Guid.NewGuid(),
                KioskId = id,
                CreatedDateTimeUtc = DateTime.UtcNow,
                Status = (int)PingStatus.Received
            });
            _kioskRepository.Update(kiosk);

            return new ServiceResult<bool>
            {
                Data = true,
                ResultCode = ResultCode.Ok,
                Message = "Ping success"
            };
        }

        public List<Kiosk> GetAll()
        {
            var kiosks = _kioskRepository.GetAll();
            return kiosks != null ? kiosks.ToList() : new List<Kiosk>();
        }
    }
}
