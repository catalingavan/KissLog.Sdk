using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace KissLog.AspNet.WebApi
{
    // usage
    // WebApiConfig.cs
    // config.Services.Replace(typeof(IHttpControllerSelector), new HttpNotFoundAwareDefaultHttpControllerSelector(config));
    // https://weblogs.asp.net/imranbaloch/handling-http-404-error-in-asp-net-web-api

    internal class HttpNotFoundAwareDefaultHttpControllerSelector : DefaultHttpControllerSelector
    {
        public HttpNotFoundAwareDefaultHttpControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {}

        public override string GetControllerName(HttpRequestMessage request)
        {
            return base.GetControllerName(request);
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            try
            {
                return base.SelectController(request);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
