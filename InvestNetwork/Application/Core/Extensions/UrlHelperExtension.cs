using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvestNetwork.Application.Core
{
    public static class UrlHelperExtension
    {
        public static string Absolute(this System.Web.Mvc.UrlHelper url, string relativeUrl)
        {
            var request = url.RequestContext.HttpContext.Request;

            return string.Format("{0}://{1}{2}",
                (request.IsSecureConnection) ? "https" : "http",
                request.Headers["Host"],
                VirtualPathUtility.ToAbsolute(relativeUrl));
        }

        public static string Absolute(this System.Web.Http.Routing.UrlHelper url, string relativeUrl)
        {
            var request = System.Web.HttpContext.Current.Request;

            return string.Format("{0}://{1}{2}",
                (request.IsSecureConnection) ? "https" : "http",
                request.Headers["Host"],
                VirtualPathUtility.ToAbsolute(relativeUrl));
        }
    }
}