using KissLog.AspNet.WebApi;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace AspNet.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // add KissLog Exception logger
            config.Services.Replace(typeof(IExceptionLogger), new KissLogExceptionLogger());

            // add KissLog exception filter
            config.Filters.Add(new KissLogWebApiExceptionFilterAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
