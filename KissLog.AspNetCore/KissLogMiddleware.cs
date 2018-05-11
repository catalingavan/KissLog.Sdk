using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KissLog.Web;

namespace KissLog.AspNetCore
{
    internal class KissLogMiddleware
    {
        private readonly RequestDelegate _next;

        public KissLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            ILogger logger = LoggerFactory.GetInstance(context);

            WebRequestProperties webRequestProperties = WebRequestPropertiesFactory.Create(context.Request);

            Exception ex = null;

            try
            {
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                ex = e;
                throw;
            }
            finally
            {
                webRequestProperties.EndDateTime = DateTime.UtcNow;

                HttpStatusCode statusCode = (HttpStatusCode)context.Response.StatusCode;

                if (ex != null)
                {
                    statusCode = HttpStatusCode.InternalServerError;
                    logger.Log(LogLevel.Error, ex);
                }

                logger.SetHttpStatusCode(statusCode);

                ResponseProperties responseProperties = ResponsePropertiesFactory.Create(context.Response);
                responseProperties.HttpStatusCode = statusCode;
                webRequestProperties.Response = responseProperties;

                ((Logger)logger).WebRequestProperties = webRequestProperties;

                IEnumerable<ILogger> loggers = LoggerFactory.GetAll(context);

                Logger.NotifyListeners(loggers.ToArray());
            }
        }
    }

    public static class KissLogMiddlewareExtensions
    {
        public static IApplicationBuilder UseKissLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<KissLogMiddleware>();
        }
    }
}
