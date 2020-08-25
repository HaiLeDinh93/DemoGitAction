using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Core.Utility;
using CarMD.Fleet.Data.EntityFramework;
using CarMD.Fleet.Data.Response;
using CarMD.Fleet.Repository.IRepository;
using CarMD.Fleet.Service.IService;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Entity = CarMD.Fleet.Repository.EntityFramework;

namespace CarMD.Fleet.Service.Service
{
    public class VimeoSettingService : IVimeoSettingService
    {
        private readonly IVimeoSettingRepository _vimeoSettingRepository;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public VimeoSettingService(IVimeoSettingRepository vimeoSettingRepository)
        {
            _vimeoSettingRepository = vimeoSettingRepository;
        }

        public VimeoSetting Get(int type = 0)
        {
            var video = _vimeoSettingRepository.GetFirstOrDefault(v => v.Type == type);
            return video;
        }
    }
}
