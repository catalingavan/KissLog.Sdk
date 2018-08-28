using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KissLog.Web;
using Microsoft.Extensions.Logging;

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

            WebRequestProperties webRequestProperties = WebRequestPropertiesFactory.Create(logger, context.Request);

            Exception ex = null;
            var originalBodyStream = context.Response.Body;
            string responseBody = null;

            try
            {
                using (var responseStream = new MemoryStream())
                {
                    context.Response.Body = responseStream;

                    await _next(context);

                    responseBody = await ReadResponse(context.Response);

                    if (context.Response?.StatusCode != (int)HttpStatusCode.NoContent)
                    {
                        await responseStream.CopyToAsync(originalBodyStream);
                    }
                }
            }
            catch (Exception e)
            {
                ex = e;
                throw;
            }
            finally
            {
                context.Response.Body = originalBodyStream;

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

                if (!string.IsNullOrEmpty(responseBody) && ShouldLogResponseBody(logger, webRequestProperties))
                {
                    string responseFileName = InternalHelpers.ResponseFileName(webRequestProperties.Response.Headers);
                    logger.LogAsFile(responseBody, responseFileName);
                }

                ((Logger)logger).WebRequestProperties = webRequestProperties;

                IEnumerable<ILogger> loggers = LoggerFactory.GetAll(context);

                Logger.NotifyListeners(loggers.ToArray());
            }
        }

        private async Task<string> ReadResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }

        private bool ShouldLogResponseBody(ILogger logger, WebRequestProperties webRequestProperties)
        {
            if (logger is Logger theLogger)
            {
                var logResponse = theLogger.GetCustomProperty(InternalHelpers.LogResponseBodyProperty);
                if (logResponse != null && logResponse is bool asBoolean)
                {
                    return asBoolean;
                }
            }

            return KissLogConfiguration.ShouldLogResponseBody(webRequestProperties);
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
