using CarMD.Fleet.Common.Configuration;
using CarMD.Fleet.Common.Enum;
using CarMD.Fleet.Common.Helper;
using CarMD.Fleet.Data.Request.Api;
using CarMD.Fleet.Data.Response.Api;
using CarMD.Fleet.Service.IService;
using CarMD.Shell.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CarMD.Shell.Api.Attributes
{
    public class ValidateInputFilterAttribute : ActionFilterAttribute
    {
        private readonly string _methodName;

        public ValidateInputFilterAttribute(string methodName)
        {
            _methodName = methodName;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var request = actionContext.HttpContext.Request;
            var response = actionContext.HttpContext.Response;

            request.HttpContext.Items["MethodCalled"] = _methodName;

            var message = string.Empty;
            var resultCode = ResultCode.Ok;
            UserModel user = null;

            var authorizationKey = request.Headers["AuthorizationKey"];
            if (!StringValues.IsNullOrEmpty(authorizationKey))
            {
                var model = AuthorizationHelper.Decrypt(authorizationKey);
                if (model == null)
                {
                    message = "Invalid Authorization Key.";
                    resultCode = ResultCode.E001002;
                }
                else
                {
                    IApiService testDriveService = actionContext.HttpContext.RequestServices.GetService(typeof(IApiService)) as IApiService;

                    var loginModel = new LoginModel
                    {
                        U = model.Email,
                        P = model.Password
                    };
                    var result = testDriveService.Login(loginModel, false);

                    if (result.ResultCode != ResultCode.Ok)
                    {
                        message = result.Message;
                        resultCode = result.ResultCode;
                    }
                    else
                    {
                        user = result.Data;
                    }
                }
            }
            else
            {
                var token = request.Headers["Auth0TokenKey"];
                if (!StringValues.IsNullOrEmpty(authorizationKey))
                {
                    var client = new RestClient($"https://{CommonConfiguration.Auth0Domain}/userinfo");
                    var request1 = new RestRequest(Method.GET);
                    request1.AddHeader("Authorization", token);
                    var response1 = client.Execute(request1);
                    var jObject = JObject.Parse(response1.Content);
                    if (response1.StatusCode == HttpStatusCode.OK)
                    {
                        var email = jObject.GetValue("name").ToString();

                        IApiService testDriveService = actionContext.HttpContext.RequestServices.GetService(typeof(IApiService)) as IApiService;

                        var result = testDriveService.LoginByAuth0(email);

                        if (result.ResultCode != ResultCode.Ok)
                        {
                            message = result.Message;
                            resultCode = result.ResultCode;
                        }
                        else
                        {
                            user = result.Data;
                        }
                    }
                    else
                    {
                        message = "Invalid Token Key.";
                        resultCode = ResultCode.E001002;
                    }
                }
                else
                {
                    message = "Authorization Key is required.";
                    resultCode = ResultCode.E001001;
                }
            }

            if (!string.IsNullOrEmpty(message) || user == null)
            {
                var result = new
                {
                    Message = ResponseHelper.CreateMessage(_methodName, request.Method, resultCode, message),
                    Data = string.Empty
                };

                var res = new JsonResult(result)
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
                actionContext.Result = res;

                return;
            }
            else
            {
                request.HttpContext.Items["UserId"] = user.Id;
                request.HttpContext.Items["UserEmail"] = user.Email;
                request.HttpContext.Items["KioskId"] = user.KioskId;
                request.HttpContext.Items["TimeZone"] = user.TimeZone;
            }

            var nullArgs = actionContext.ActionArguments.Where(kv => kv.Value == null).Select(kv => string.Format("The argument '{0}' cannot be null", kv.Key)).ToArray();

            if (nullArgs.Any())
            {
                var result = new
                {
                    Message = ResponseHelper.CreateMessage(_methodName, request.Method, ResultCode.DataInvaild, string.Join(" ", nullArgs)),
                    Data = string.Empty
                };
                actionContext.Result = new BadRequestObjectResult(result);

                return;
            }

            if (!actionContext.ModelState.IsValid)
            {
                var errorList = new List<string>();
                var errors = actionContext.ModelState.Where(keyValue => keyValue.Value.Errors.Any());
                foreach (var keyValue in errors)
                {
                    var error = keyValue.Value.Errors.FirstOrDefault(e => !string.IsNullOrEmpty(e.ErrorMessage) || e.Exception != null);
                    if (error != null)
                    {
                        var errorMessage = !string.IsNullOrEmpty(error.ErrorMessage) ? error.ErrorMessage : error.Exception.Message;
                        errorList.Add(errorMessage);
                    }
                }

                message = "Invalid input data.";
                if (errorList.Any())
                {
                    message = string.Join(" ", errorList);
                }

                var result = new
                {
                    Message = ResponseHelper.CreateMessage(_methodName, request.Method, ResultCode.DataInvaild, message),
                    Data = string.Empty
                };
                actionContext.Result = new BadRequestObjectResult(result);
                return;
            }
        }

    }
}
