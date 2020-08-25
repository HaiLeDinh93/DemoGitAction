using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Data.Response;
using CarMD.Fleet.Service.IService;
using CarMD.Shell.Api.Attributes;
using CarMD.Shell.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;

namespace CarMD.Shell.Api.Controllers
{
    [ApiController]
    [UnhandledExceptionFilter]
    public class BaseApiController : ControllerBase
    {
        protected readonly IApiService _apiService;

        public BaseApiController(IApiService apiService)
        {
            _apiService = apiService;
        }

        //Required ValidateInputFilter to parse user Id from Authorization Key
        public Guid UserId
        {
            get
            {
                var userId = Request.HttpContext.Items["UserId"];
                return userId == null ? Guid.Empty : (Guid)userId;
            }
        }

        public string UserEmail
        {
            get
            {
                var userEmail = Request.HttpContext.Items["UserEmail"];
                return (string)userEmail;
            }
        }

        public long KioskId
        {
            get
            {
                var kioskId = Request.HttpContext.Items["KioskId"];
                return kioskId == null ? 0 : (long)kioskId;
            }
        }

        public string TimeZone
        {
            get
            {
                var timeZone = Request.HttpContext.Items["TimeZone"];
                return timeZone == null ? "Pacific Standard Time" : (string)timeZone;
            }
        }

        internal ContentResult CreateJsonResponse<T>(ServiceResult<T> sourceResult)
        {
            return CreateResponse(CreateMessage(sourceResult.ResultCode, sourceResult.Data, sourceResult.Message));
        }

        internal ContentResult CreateJsonResponse<T>(HttpStatusCode status, ServiceResult<T> sourceResult)
        {
            return CreateResponse(status, CreateMessage(sourceResult.ResultCode, sourceResult.Data, sourceResult.Message));
        }

        private ContentResult CreateResponse<T>(T jsonData)
        {
            var resp = new ContentResult
            {
                StatusCode = (int)HttpStatusCode.OK,
                ContentType = "application/json"
            };

            var result = JsonConvert.SerializeObject(jsonData,
                                                     Formatting.None,
                                                     new JsonSerializerSettings
                                                     {
                                                         ContractResolver
                                                                 = new CamelCasePropertyNamesContractResolver()
                                                     });

            resp.Content = result;
            return resp;
        }

        private ContentResult CreateResponse<T>(HttpStatusCode status, T jsonData)
        {
            var resp = new ContentResult
            {
                StatusCode = (int)status,
                ContentType = "application/json"
            };

            var result = JsonConvert.SerializeObject(jsonData,
                                                     Formatting.None,
                                                     new JsonSerializerSettings
                                                     {
                                                         ContractResolver
                                                                 = new CamelCasePropertyNamesContractResolver()
                                                     });
            resp.Content = result;
            return resp;
        }

        private object CreateMessage<T>(ResultCode code, T jsonData, string error = "")
        {
            var message = string.IsNullOrWhiteSpace(error) ? new MessageBase(code) : new MessageBase(code, error);

            message.Version = Globals.ApiVersion;

            message.Method = Request.HttpContext.Items["MethodCalled"] as string;

            message.Action = Request.Method;

            return new
            {
                Message = message,
                Data = jsonData
            };
        }

    }
}