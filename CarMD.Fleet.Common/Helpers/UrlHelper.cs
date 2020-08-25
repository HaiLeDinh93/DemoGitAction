using System;
using System.Collections.Generic;
using System.Text;

namespace CarMD.Fleet.Common.Helpers
{
    public static class UrlHelper
    {
        public static string Merge(string baseUrl, string relativeUri = "")
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                return baseUrl;
            }
            var baseUri = new Uri(baseUrl);
            if (string.IsNullOrEmpty(relativeUri))
            {
                return baseUri.ToString();
            }
            while (relativeUri.StartsWith("/"))
            {
                relativeUri = relativeUri.Substring(1);
            }
            return new Uri(baseUri, relativeUri).ToString();
        }
    }
}
