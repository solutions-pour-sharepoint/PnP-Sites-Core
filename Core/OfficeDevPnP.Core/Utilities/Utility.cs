using System;
using System.Linq.Expressions;
using Microsoft.SharePoint.Client;
using System.Net;

namespace OfficeDevPnP.Core.Utilities
{
    /// <summary>
    /// Holds a method that returns health score for a SharePoint Server
    /// </summary>
    public static partial class Utility
    {
        /// <summary>
        /// Returns the healthscore for a SharePoint Server
        /// </summary>
        /// <param name="url">SharePoint server URL</param>
        /// <returns>Returns server health score integer value</returns>
        public static int GetHealthScore(string url)
        {
            var value = 0;
            var baseUri = new Uri(url);
            var checkUri = new Uri(baseUri, "_layouts/15/blank.htm");
            var webRequest = WebRequest.Create(checkUri);
            webRequest.Method = "HEAD";
            webRequest.UseDefaultCredentials = true;
            using (WebResponse webResponse = webRequest.GetResponse())
            {
                foreach (string header in webResponse.Headers)
                {
                    if (header == "X-SharePointHealthScore")
                    {
                        value = Int32.Parse(webResponse.Headers[header]);
                        break;
                    }
                }
            }
            return value;
        }
    }
}
