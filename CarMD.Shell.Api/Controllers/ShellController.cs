using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Data.EntityFramework;
using CarMD.Fleet.Data.Request;
using CarMD.Fleet.Data.Request.Api;
using CarMD.Fleet.Data.Request.Kiosk;
using CarMD.Fleet.Data.Response;
using CarMD.Fleet.Service.IService;
using CarMD.Shell.Api.Attributes;
using CarMD.Shell.Api.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;

namespace CarMD.Shell.Api.Controllers
{
    public class ShellController : BaseApiController
    {
        public readonly IUserService _userService;
        public readonly IKioskService _kioskService;
        public readonly IVimeoSettingService _vimeoSettingService;

        public ShellController(IApiService apiService, IUserService userService, IKioskService kioskService, IVimeoSettingService vimeoSettingService) : base(apiService)
        {
            _userService = userService;
            _kioskService = kioskService;
            _vimeoSettingService = vimeoSettingService;
        }

        [HttpPost("Login")]
        [ValidateAppFilter("Login")]
        public ContentResult Login([FromBody] LoginModel loginModel)
        {
            var result = _apiService.Login(loginModel);
            return CreateJsonResponse(result);
        }

        [HttpPost("ResetPassword/{email}")]
        [ValidateAppFilter("ResetPassword")]
        public ContentResult ResetPassword(string email)
        {
            var result = _userService.ResetPassword(email);
            return CreateJsonResponse(result);
        }

        [HttpGet("UserInfo")]
        [ValidateInputFilter("GetUserInformation")]
        public ContentResult GetUserInformation()
        {
            var result = _apiService.GetUserInformation(UserId);
            return CreateJsonResponse(result);
        }

        [HttpPost("Logs/Time")]
        [ValidateInputFilter("LogTime")]
        public ContentResult UploadLogsTime([FromBody]LoggingTimeModel model)
        {
            var result = _apiService.CreateLoggingTime(UserId, KioskId, model);
            return CreateJsonResponse(result);
        }

        [HttpPost("Logs/Error")]
        [ValidateInputFilter("LogError")]
        public ContentResult UploadLogsError([FromBody]LoggingErrorModel model)
        {
            var result = _apiService.CreateLoggingError(UserId, KioskId, model);
            return CreateJsonResponse(result);
        }

        [HttpGet("Logs/Time")]
        [ValidateInputFilter("SearchLogsTime")]
        public ContentResult SearchLogsTime([FromQuery] int pageSize = 20, [FromQuery] int pageIndex = 1)
        {
            var searchModel = new SearchCriteria(pageIndex, pageSize);
            searchModel.SortDirection = SortDirection.Desc;
            searchModel.SortColumn = "StartTime";
            var result = _apiService.SearchLoggingTime(searchModel);
            return CreateJsonResponse(result);
        }

        [HttpGet("Logs/Error")]
        [ValidateInputFilter("SearchLogsError")]
        public ContentResult SearchLogsError([FromQuery] int pageSize = 20, [FromQuery] int pageIndex = 1)
        {
            var searchModel = new SearchCriteria(pageIndex, pageSize);
            searchModel.SortDirection = SortDirection.Desc;
            searchModel.SortColumn = "CreatedDate";
            var result = _apiService.SearchLoggingError(searchModel);
            return CreateJsonResponse(result);
        }

        [HttpPost("Emails/MissingDongle")]
        [ValidateInputFilter("MissingDongle")]
        public ContentResult MissingDongle(MissingDongle missingDongle)
        {
            var toList = Globals.MissingDongleEmails.Split(';');
            var to = new List<string>();
            foreach (var item in toList)
            {
                to.Add(item);
            }

            var model = new FeedBackModel
            {
                KioskId = KioskId,
                UserId = UserId,
                UserEmail = UserEmail,
                CustomerEmail = missingDongle.CustomerEmail,
                ToEmails = to,
                Type = (int)FeedBackType.MissingDongle,
            };
            var result = _apiService.FeedBack(model);
            return CreateJsonResponse(result);
        }

        [HttpPost("Emails/NotifyHavingDongle")]
        [ValidateInputFilter("NotifyHavingDongle")]
        public ContentResult NotifyHavingDongle()
        {
            var result = _apiService.NotifyHavingDongle(KioskId);
            return CreateJsonResponse(result);
        }

        [HttpPost("Emails/Lockbox")]
        [ValidateInputFilter("Lockbox")]
        public ContentResult Lockbox()
        {
            var toList = Globals.LockboxEmails.Split(';');
            var to = new List<string>();
            foreach (var item in toList)
            {
                to.Add(item);
            }
            var model = new FeedBackModel
            {
                KioskId = KioskId,
                UserId = UserId,
                UserEmail = UserEmail,
                Type = (int)FeedBackType.Lockbox,
                ToEmails = to
            };
            var result = _apiService.FeedBack(model);
            return CreateJsonResponse(result);
        }

        [HttpPost("Emails/ReportFeedBack")]
        [ValidateInputFilter("ReportFeedBack")]
        public ContentResult ReportFeedBack(ReportFeedBack feedBack)
        {
            var toList = Globals.FeedbackEmails.Split(';');
            var to = new List<string>();
            foreach (var item in toList)
            {
                to.Add(item);
            }
            var model = new FeedBackModel
            {
                KioskId = KioskId,
                UserId = UserId,
                UserEmail = UserEmail,
                CustomerEmail = feedBack.CustomerEmail,
                LikeThis = feedBack.LikeThis,
                Comment = feedBack.Comment,
                ReportId = feedBack.ReportId,
                Type = (int)FeedBackType.Report,
                ToEmails = to
            };
            var result = _apiService.FeedBack(model);
            return CreateJsonResponse(result);
        }

        [HttpPut("Kiosk/Address")]
        [ValidateInputFilter("UpdateKioskAddress")]
        public ContentResult UpdateKioskAddress([FromBody] UpdateKioskModel model)
        {
            model.Id = KioskId;

            var result = _kioskService.Update(model);
            return CreateJsonResponse(result);
        }

        [HttpGet("VimeoSetting")]
        [ValidateInputFilter("VimeoSetting")]
        public ContentResult VimeoSetting(int type = 0)
        {
            var video = _vimeoSettingService.Get(type);

            if (video != null)
            {
                return CreateJsonResponse(new ServiceResult<VimeoSetting>
                {
                    Message = "Get Vimeo Setting",
                    Data = video,
                    ResultCode = ResultCode.Ok
                });
            }

            return CreateJsonResponse(new ServiceResult<VimeoSetting>
            {
                Message = "Vimeo Setting Not Found",
                ResultCode = ResultCode.DataInvaild
            });
        }

        [HttpPost("Ping/{id}")]
        [ValidateAppFilter("Ping")]
        public ContentResult Ping(long id)
        {
            var result = _kioskService.Ping(id);
            return CreateJsonResponse(result);
        }

    }
}