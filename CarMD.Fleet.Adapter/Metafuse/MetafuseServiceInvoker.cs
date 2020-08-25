using MetafuseReference;
using CarMD.Fleet.Core.Exceptions;
using log4net;
using System;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.Options;
using CarMD.Fleet.Common.Configuration;
using CarMD.Fleet.Core.Utility;

namespace CarMD.Fleet.Adapter.Metafuse
{
    internal sealed class MetafuseServiceInvoker : ServiceV6SoapClient, IWebServiceInvoker
    {
        
        public static int WebServiceRetryCount = String.IsNullOrEmpty(AppSettingUtility.Settings["WebServiceRetryCount"])
                                                     ? 1
                                                     : Int32.Parse(AppSettingUtility.Settings["WebServiceRetryCount"]);
        public string LanguageString = "es".Equals(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName)
                                           ? "es-MX"
                                           : "en-US";

        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public static WebServiceKey ServiceKey(string webServiceKey)
        {
            return new WebServiceKey
            {
                Key = webServiceKey,
                LanguageString = "en-US"
            };
        }

        public MetafuseServiceInvoker() : base(EndpointConfiguration.ServiceV6Soap, AppSettingUtility.Settings["MetafuseWebServiceUrl"])
        
        {

        }
         
        /// <summary>
        /// call webservice with n times retry
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="serviceOperator">method to call</param>
        /// <param name="param">params of method</param>
        /// <returns></returns>
        public TResult InvokeWebService<TResult>(string serviceOperator, params object[] param)
        {
            Type type = GetType();
            MethodInfo method = type.GetMethod(serviceOperator);

            for (int retry = 0; retry < WebServiceRetryCount; ++retry)
            {
                try
                {
                    return (TResult)method.Invoke(this, param);
                }
                catch (TargetInvocationException exp)
                {
                    if (retry == WebServiceRetryCount - 1)
                    {
                        _logger.Fatal("Metafuse service exception: ", exp);
                        throw new MetafuseServiceUnavailableException("Metafuse webservice is unavailable.", exp);
                    }
                }
                catch (Exception e)
                {
                    _logger.Fatal("[REPORTPARAMS] :" + String.Join(" - ", param));
                    if (retry == WebServiceRetryCount - 1)
                    {
                        _logger.Fatal("Metafuse service exception: ", e);
                        throw new MetafuseServiceUnavailableException("Exceeded maximum retry request.", e);
                    }
                }
            }
            return default(TResult);
        }
    }
}
