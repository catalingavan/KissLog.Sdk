using KissLog.Internal;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

            KissLog.Web.WebProperties webProperties = new KissLog.Web.WebProperties
            {
                Request = HttpRequestFactory.Create(context.Request)
            };

            logger.DataContainer.WebProperties = webProperties;

            KissLog.Internal.NotifyListeners.NotifyBeginRequest(webProperties.Request, logger);

            Exception ex = null;
            Stream originalBodyStream = context.Response.Body;
            TemporaryFile responseBodyFile = null;
            long contentLength = 0;

            try
            {
                using (var responseStream = new MemoryStream())
                {
                    context.Response.Body = responseStream;

                    await _next(context);

                    responseBodyFile = await ReadResponseAsync(context.Response);

                    contentLength = responseStream.Length;

                    if (CanWriteToResponseBody(context.Response))
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

                KissLog.Web.HttpResponse response = HttpResponseFactory.Create(context.Response);
                webProperties.Response = response;

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
                
                response.HttpStatusCode = statusCode;
                response.Properties.ContentLength = contentLength;

                if(responseBodyFile != null && InternalHelpers.PreFilterShouldLogResponseBody(logger, responseBodyFile, response.Properties))
                {
                    string responseFileName = InternalHelpers.ResponseFileName(response.Properties.Headers);
                    logger.LogFile(responseBodyFile.FileName, responseFileName);
                }

                logger.DataContainer.WebProperties = webProperties;

                responseBodyFile?.Dispose();

                IEnumerable<ILogger> loggers = Logger.Factory.GetAll();

                Logger.NotifyListeners(loggers.ToArray());
            }
        }

        private async Task<TemporaryFile> ReadResponseAsync(HttpResponse response)
        {
            TemporaryFile responseBodyFile = null;

            try
            {
                responseBodyFile = new TemporaryFile();

                response.Body.Seek(0, SeekOrigin.Begin);

                using (var fs = File.OpenWrite(responseBodyFile.FileName))
                {
                    await response.Body.CopyToAsync(fs);
                }
            }
            catch(Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("KissLogMiddleware.ReadResponseAsync error");
                sb.AppendLine(ex.ToString());

                KissLog.Internal.InternalHelpers.Log(sb.ToString(), LogLevel.Error);

                if(responseBodyFile != null)
                {
                    responseBodyFile.Dispose();
                }

                responseBodyFile = null;
            }
            finally
            {
                response.Body.Seek(0, SeekOrigin.Begin);
            }

            return responseBodyFile;
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
}
