using KissLog.LogResponseBody;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace KissLog.AspNetCore
{
    internal class KissLogMiddleware
    {
        static KissLogMiddleware()
        {
            KissLog.InternalHelpers.WrapInTryCatch(() =>
            {
                ModuleInitializer.Init();
            });
            
            Logger.Factory = new LoggerFactory();
        }

        private readonly RequestDelegate _next;
        public KissLogMiddleware(RequestDelegate next)
        {
            Logger.Factory = new LoggerFactory();

            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            KissLog.Http.HttpRequest httpRequest = HttpRequestFactory.Create(context.Request);

            var factory = new LoggerFactory();
            Logger logger = factory.GetInstance(context);
            logger.DataContainer.SetHttpProperties(new Http.HttpProperties(httpRequest));

            KissLog.InternalHelpers.WrapInTryCatch(() =>
            {
                NotifyListeners.NotifyBeginRequest.Notify(httpRequest);
            });

            ExceptionDispatchInfo ex = null;
            
            context.Response.Body = new MirrorStreamDecorator(context.Response.Body);

            try
            {
                await _next(context);
            }
            catch(Exception e)
            {
                ex = ExceptionDispatchInfo.Capture(e);
                throw;
            }
            finally
            {
                MirrorStreamDecorator responseStream = GetResponseStream(context.Response);
                long contentLength = responseStream == null ? 0 : responseStream.MirrorStream.Length;
                int statusCode = context.Response.StatusCode;

                if (ex != null)
                {
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    logger.Error(ex.SourceException);
                }

                KissLog.Http.HttpResponse httpResponse = HttpResponseFactory.Create(context.Response, contentLength);
                httpResponse.SetStatusCode(statusCode);

                logger.DataContainer.HttpProperties.SetResponse(httpResponse);

                if (responseStream != null)
                {
                    if (KissLog.InternalHelpers.CanReadResponseBody(httpResponse.Properties.Headers))
                    {
                        if (ShouldLogResponseBody(logger, factory, context))
                        {
                            ILogResponseBodyStrategy logResponseBody = LogResponseBodyStrategyFactory.Create(responseStream.MirrorStream, responseStream.Encoding, logger);
                            logResponseBody.Execute();
                        }
                    }

                    responseStream.MirrorStream.Dispose();
                }

                IEnumerable<Logger> loggers = factory.GetAll(context);

                KissLog.InternalHelpers.WrapInTryCatch(() =>
                {
                    NotifyListeners.NotifyFlush.Notify(loggers.ToArray());
                });
            }
        }

        private MirrorStreamDecorator GetResponseStream(HttpResponse response)
        {
            if (response.Body != null && response.Body is MirrorStreamDecorator stream)
            {
                if (!stream.MirrorStream.CanRead)
                    return null;

                return stream;
            }

            return null;
        }

        private bool ShouldLogResponseBody(Logger logger, LoggerFactory loggerFactory, HttpContext httpContext)
        {
            var loggers = loggerFactory.GetAll(httpContext);

            bool? explicitValue = KissLog.InternalHelpers.GetExplicitLogResponseBody(loggers);

            if (explicitValue.HasValue)
                return explicitValue.Value;

            return KissLogConfiguration.Options.Handlers.ShouldLogResponseBody.Invoke(logger.DataContainer.HttpProperties);
        }
    }
}
