using KissLog.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
            (logger as Logger)?.AddCustomProperty(InternalHelpers.IsCreatedByHttpRequest, true);

            WebRequestProperties webRequestProperties = WebRequestPropertiesFactory.Create(logger, context.Request);

            Exception ex = null;
            Stream originalBodyStream = context.Response.Body;
            TemporaryFile responseBodyFile = null;

            try
            {
                using (var responseStream = new MemoryStream())
                {
                    context.Response.Body = responseStream;

                    await _next(context);

                    responseBodyFile = new TemporaryFile();
                    await ReadResponse(context.Response, responseBodyFile.FileName);

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

                if (responseBodyFile != null && ShouldLogResponseBody(logger, webRequestProperties))
                {
                    string responseFileName = InternalHelpers.ResponseFileName(webRequestProperties.Response.Headers);
                    logger.LogFile(responseBodyFile.FileName, responseFileName);
                }

                ((Logger)logger).SetWebRequestProperties(webRequestProperties);

                responseBodyFile?.Dispose();

                IEnumerable<ILogger> loggers = LoggerFactory.GetAll(context);

                Logger.NotifyListeners(loggers.ToArray());
            }
        }

        private async Task ReadResponse(HttpResponse response, string destinationFilePath)
        {
            try
            {
                response.Body.Seek(0, SeekOrigin.Begin);

                using (var fs = File.OpenWrite(destinationFilePath))
                {
                    await response.Body.CopyToAsync(fs);
                }
            }
            finally
            {
                response.Body.Seek(0, SeekOrigin.Begin);
            }
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
        static KissLogMiddlewareExtensions()
        {
            PackageInit.Init();
        }

        public static IApplicationBuilder UseKissLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<KissLogMiddleware>();
        }
    }
}
