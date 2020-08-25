using System;
using System.Net;
using System.Runtime.Caching;
using CarMD.Fleet.Data.Request.Api;
using CarMD.Shell.Api.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using CarMD.Fleet.Common.Enum;
using Microsoft.AspNetCore.Mvc;

namespace CarMD.Fleet.Vehimatics.Api.Attributes
{
    public class ThrottleCreateReportFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The number of seconds clients must wait before executing this decorated route again.
        /// </summary>
        public int Seconds { get; set; }


        public ThrottleCreateReportFilterAttribute(int seconds)
        {
            Seconds = seconds;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var key = "CreateReport_";

            var parameters = actionContext.ActionDescriptor.Parameters;

            if (parameters != null)
            {
                var p = parameters[0];
                if (p != null && "VehicleDataModel".Equals(p.Name, StringComparison.OrdinalIgnoreCase))
                {
                    var val = actionContext.ActionArguments[p.Name] as VehicleDataModel;
                    if (val != null)
                    {
                        key = key + val.Vin;
                    }
                }
            }

            var allowExecute = false;

            if (MemoryCache.Default.Get(key) == null)
            {
                MemoryCache.Default.Add(key,
                    true, 
                    DateTime.Now.AddSeconds(Seconds));
                allowExecute = true;
            }

            if (!allowExecute)
            {
                var message = "You may only perform this action every " + Seconds + " seconds.";

                var result = new
                {
                    Message = ResponseHelper.CreateMessage("CreateReport", "POST", ResultCode.Error, message),
                    Data = string.Empty
                };

                var response = new JsonResult(result)
                {
                    StatusCode = (int)HttpStatusCode.Conflict
                };
                actionContext.Result = response;

                return;
            }
        }
    }
}