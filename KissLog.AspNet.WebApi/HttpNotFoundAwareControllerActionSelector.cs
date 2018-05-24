using System.Web.Http;
using System.Web.Http.Controllers;

namespace KissLog.AspNet.WebApi
{
    // usage
    // WebApiConfig.cs
    // config.Services.Replace(typeof(IHttpActionSelector), new HttpNotFoundAwareControllerActionSelector());
    // https://weblogs.asp.net/imranbaloch/handling-http-404-error-in-asp-net-web-api

    internal class HttpNotFoundAwareControllerActionSelector : ApiControllerActionSelector
    {
        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            try
            {
                return base.SelectAction(controllerContext);
            }
            catch (HttpResponseException ex)
            {
                var x = 1;
                throw;
            }
        }
    }
}
