using CarMD.Fleet.Common.Enum;
using CarMD.Shell.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CarMD.Shell.Api.Attributes
{
    public class ValidateAppFilterAttribute : ActionFilterAttribute
    {
        private readonly string _methodName;

        public ValidateAppFilterAttribute(string methodName)
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

            var authorizationKey = actionContext.HttpContext.Request.Headers["ApiKey"];
            if (!StringValues.IsNullOrEmpty(authorizationKey))
            {
                if (!Globals.ApiKey.Equals(authorizationKey, System.StringComparison.OrdinalIgnoreCase))
                {
                    message = "Invalid Shell Key.";
                    resultCode = ResultCode.E001002;
                }
            }
            else
            {
                message = "Shell Key is required.";
                resultCode = ResultCode.E001001;
            }

            if (!string.IsNullOrEmpty(message))
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
            }
        }


    }
}
