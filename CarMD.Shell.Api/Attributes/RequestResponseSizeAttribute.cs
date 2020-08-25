using System;
using System.Runtime.Caching;
using Microsoft.AspNetCore.Mvc.Filters;
using log4net;
using System.Reflection;

namespace CarMD.Fleet.Vehimatics.Api.Attributes
{
    public class RequestResponseSizeAttribute : ActionFilterAttribute
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The number of seconds clients must wait before executing this decorated route again.
        /// </summary>
        public string Step { get; set; }

        public RequestResponseSizeAttribute(string step)
        {
            Step = step;
        }

        public override void OnResultExecuted(ResultExecutedContext actionContext)
        {
            var request = actionContext.HttpContext.Request;
            var response = actionContext.HttpContext.Response;
            var user = response.HttpContext.Items["UserEmail"];

            var key = "CreateReport_" + user;

            var headers = request.Headers;
            var count = 60;
            foreach (var item in headers)
            {
                count = count + item.Key.Length + item.Value.ToString().Length;
            }

            var lengthRequest = (request.ContentLength ?? 0) + count;

            var lengthResponse = (response.ContentLength ?? 0) + 160;

            var length = lengthRequest + lengthResponse;

            if (MemoryCache.Default.Get(key) == null)
            {
                MemoryCache.Default.Add(key,
                    length,
                    DateTime.Now.AddSeconds(600));
            }
            else
            {
                var total = (long)MemoryCache.Default.Get(key) + length;

                MemoryCache.Default.Set(key,
                    total,
                    DateTime.Now.AddSeconds(600));
            }

            _logger.Info("CreateReport - " + user + " - Step: " + Step + " - Request: " + lengthRequest + " Bytes" + " - Response: " + lengthResponse + " Bytes" + " - Total Steps: " + length + " Bytes");

            if ("CreateReport".Equals(Step, StringComparison.OrdinalIgnoreCase) || "CreateInnovaReport".Equals(Step, StringComparison.OrdinalIgnoreCase))
            {
                _logger.Info("CreateReport - " + user + " - Total Process: " + MemoryCache.Default.Get(key) + " Bytes");

                MemoryCache.Default.Set(key,
                      0,
                      DateTime.Now.AddSeconds(1));             
            }           
        }
    }
}