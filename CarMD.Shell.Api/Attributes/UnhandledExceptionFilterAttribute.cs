using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Reflection;
using ExceptionFilterAttribute = Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute;

namespace CarMD.Shell.Api.Attributes
{
    public class UnhandledExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnException(ExceptionContext context)
        {
            string stack = context.Exception.StackTrace;

            var apiError = new Exception("There is an error. Please contact to admin for support.");

            var response = new JsonResult(apiError)
            {
                StatusCode = 500
            };
            context.Result = response;

            base.OnException(context);

            if (null != context.Exception)
            {
                _logger.Fatal("Unhandled Exception Filter: ", context.Exception);
            }
            else
            {
                _logger.Fatal(stack);
            }
        }

    }
}
