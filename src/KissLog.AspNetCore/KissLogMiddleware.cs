using KissLog.Internal;
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
            Logger logger = Logger.Factory.Get() as Logger;
            if(logger == null)
                return;

            logger.DataContainer.AddProperty(InternalHelpers.IsCreatedByHttpRequest, true);

            WebRequestProperties properties = WebRequestPropertiesFactory.Create(context.Request);

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

                    if(CanWriteToResponseBody(context.Response))
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

                properties.EndDateTime = DateTime.UtcNow;

                HttpStatusCode statusCode = (HttpStatusCode)context.Response.StatusCode;

                if (ex != null)
                {
                    statusCode = HttpStatusCode.InternalServerError;
                    logger.Log(LogLevel.Error, ex);
                }

                if (logger.DataContainer.ExplicitHttpStatusCode.HasValue)
                {
                    statusCode = logger.DataContainer.ExplicitHttpStatusCode.Value;
                }

                ResponseProperties response = ResponsePropertiesFactory.Create(context.Response);
                response.HttpStatusCode = statusCode;
                properties.Response = response;

                if(responseBodyFile != null && InternalHelpers.ShouldLogResponseBody(logger, response))
                {
                    string responseFileName = InternalHelpers.ResponseFileName(response.Headers);
                    logger.LogFile(responseBodyFile.FileName, responseFileName);
                }

                logger.DataContainer.WebRequestProperties = properties;

                responseBodyFile?.Dispose();

                IEnumerable<ILogger> loggers = Logger.Factory.GetAll();

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

        private bool CanWriteToResponseBody(HttpResponse response)
        {
            if (response == null)
                return false;

            if(response.StatusCode < 200 ||
               response.StatusCode == (int)HttpStatusCode.NoContent ||
               response.StatusCode == (int)HttpStatusCode.NotModified)
            {
                return false;
            }

            return true;
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
