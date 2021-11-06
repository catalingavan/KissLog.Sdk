using KissLog.AspNet.Web.Exceptions;
using KissLog.Exceptions;
using KissLog.LogResponseBody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace KissLog.AspNet.Web
{
    public class KissLogHttpModule : IHttpModule
    {
        internal const string SessionKey = "X-KissLog-Sesssion";

        public KissLogHttpModule()
        {
            Logger.Factory = new LoggerFactory();
        }
        static KissLogHttpModule()
        {
            Logger.Factory = new LoggerFactory();
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
            context.EndRequest += EndRequest;
            context.Error += OnError;
            context.PreRequestHandlerExecute += PreRequestHandlerExecute;
            context.PostAuthenticateRequest += PostAuthenticateRquest;
            context.PostAcquireRequestState += PostAcquireRequestState;
        }

        private void BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
                return;

            HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);
            OnBeginRequest(httpContext);
        }

        private void EndRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
                return;

            HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);
            OnEndRequest(httpContext);
        }

        private void OnError(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
                return;

            HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);
            OnError(httpContext);
        }

        private void PreRequestHandlerExecute(object sender, EventArgs e)
        {
            var application = sender as HttpApplication;
            if (application == null || application.Context == null)
                return;

            HttpContextBase httpContext = new HttpContextWrapper(application.Context);
            OnPreRequestHandlerExecute(httpContext);
        }

        private void PostAuthenticateRquest(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
                return;

            HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);
            OnPostAuthenticateRquest(httpContext);
        }

        private void PostAcquireRequestState(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
                return;

            HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);
            PostAcquireRequestState(httpContext);
        }

        internal void OnBeginRequest(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            if (httpContext.Request == null)
            {
                InternalLogger.LogException(new NullHttpRequestException(nameof(OnBeginRequest)));
                return;
            }

            KissLog.Http.HttpRequest httpRequest = HttpRequestFactory.Create(httpContext.Request);

            var factory = new LoggerFactory();
            Logger logger = factory.GetInstance(httpContext);
            logger.DataContainer.SetHttpProperties(new Http.HttpProperties(httpRequest));

            KissLog.InternalHelpers.WrapInTryCatch(() =>
            {
                NotifyListeners.NotifyBeginRequest.Notify(httpRequest);
            });
        }

        internal void OnEndRequest(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            if (httpContext.Response == null)
            {
                InternalLogger.LogException(new NullHttpResponseException(nameof(OnEndRequest)));
                return;
            }

            if (httpContext.Request == null)
            {
                InternalLogger.LogException(new NullHttpRequestException(nameof(OnEndRequest)));
                return;
            }

            var factory = new LoggerFactory();
            Logger logger = factory.GetInstance(httpContext);

            // IIS redirect bypasses the IHttpModule.BeginRequest event
            if (logger.DataContainer.HttpProperties == null)
            {
                KissLog.Http.HttpRequest httpRequest = HttpRequestFactory.Create(httpContext.Request);
                logger.DataContainer.SetHttpProperties(new Http.HttpProperties(httpRequest));
            }

            MirrorStreamDecorator responseStream = GetResponseStream(httpContext.Response);
            long contentLength = responseStream == null ? 0 : responseStream.MirrorStream.Length;

            KissLog.Http.HttpResponse httpResponse = HttpResponseFactory.Create(httpContext.Response, contentLength);
            logger.DataContainer.HttpProperties.SetResponse(httpResponse);

            Exception ex = httpContext.Server?.GetLastError();
            if (ex != null)
                logger.Error(ex);

            if(responseStream != null)
            {
                if (KissLog.InternalHelpers.CanReadResponseBody(httpResponse.Properties.Headers))
                {
                    if(ShouldLogResponseBody(logger, factory, httpContext))
                    {
                        ILogResponseBodyStrategy logResponseBody = LogResponseBodyStrategyFactory.Create(responseStream.MirrorStream, responseStream.Encoding, logger);
                        logResponseBody.Execute();
                    }
                }

                responseStream.MirrorStream.Dispose();
            }

            IEnumerable<Logger> loggers = factory.GetAll(httpContext);

            KissLog.InternalHelpers.WrapInTryCatch(() =>
            {
                NotifyListeners.NotifyFlush.Notify(loggers.ToArray());
            });
        }

        internal void OnError(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            if (httpContext.Server == null)
                return;

            Exception ex = httpContext.Server.GetLastError();
            if(ex != null)
            {
                var factory = new LoggerFactory();
                Logger logger = factory.GetInstance(httpContext);

                logger.Error(ex);
            }
        }

        private MirrorStreamDecorator GetResponseStream(HttpResponseBase response)
        {
            if (response.Filter != null && response.Filter is MirrorStreamDecorator stream)
            {
                if (!stream.MirrorStream.CanRead)
                    return null;

                return stream;
            }

            return null;
        }

        internal void OnPreRequestHandlerExecute(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            if(httpContext.Response == null)
            {
                InternalLogger.LogException(new NullHttpResponseException(nameof(OnPreRequestHandlerExecute)));
                return;
            }

            if(httpContext.Response.Filter == null)
            {
                InternalLogger.LogException(new NullResponseFilterException(nameof(OnPreRequestHandlerExecute)));
                return;
            }

            httpContext.Response.Filter = new MirrorStreamDecorator(httpContext.Response.Filter, httpContext.Response.ContentEncoding);
        }

        internal void OnPostAuthenticateRquest(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            var factory = new LoggerFactory();
            Logger logger = factory.GetInstance(httpContext);

            if (logger.DataContainer.HttpProperties == null && httpContext.Request != null)
            {
                KissLog.Http.HttpRequest httpRequest = HttpRequestFactory.Create(httpContext.Request);
                logger.DataContainer.SetHttpProperties(new Http.HttpProperties(httpRequest));
            }

            if (logger.DataContainer.HttpProperties == null)
                return;

            bool isAuthenticated = httpContext.User?.Identity?.IsAuthenticated ?? false;

            logger.DataContainer.HttpProperties.Request.SetIsAuthenticated(isAuthenticated);

            if (httpContext.User == null || httpContext.User is ClaimsPrincipal == false)
                return;

            ClaimsPrincipal claimsPrincipal = httpContext.User as ClaimsPrincipal;

            if (claimsPrincipal.Identity == null || claimsPrincipal.Identity is ClaimsIdentity == false)
                return;

            ClaimsIdentity claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;

            List<KeyValuePair<string, string>> claims = InternalHelpers.ToKeyValuePair(claimsIdentity);

            logger.DataContainer.HttpProperties.Request.Properties.SetClaims(claims);
        }

        internal void PostAcquireRequestState(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            if (httpContext.Session == null)
                return;

            httpContext.Session.Add(SessionKey, true);

            var factory = new LoggerFactory();
            Logger logger = factory.GetInstance(httpContext);

            if (logger.DataContainer.HttpProperties == null && httpContext.Request != null)
            {
                KissLog.Http.HttpRequest httpRequest = HttpRequestFactory.Create(httpContext.Request);
                logger.DataContainer.SetHttpProperties(new Http.HttpProperties(httpRequest));
            }

            if (logger.DataContainer.HttpProperties == null)
                return;

            logger.DataContainer.HttpProperties.Request.SetSession(httpContext.Session.SessionID, httpContext.Session.IsNewSession);
        }

        private bool ShouldLogResponseBody(Logger logger, LoggerFactory loggerFactory, HttpContextBase httpContext)
        {
            var loggers = loggerFactory.GetAll(httpContext);

            bool? explicitValue = KissLog.InternalHelpers.GetExplicitLogResponseBody(loggers);

            if (explicitValue.HasValue)
                return explicitValue.Value;

            return KissLogConfiguration.Options.Handlers.ShouldLogResponseBody.Invoke(logger.DataContainer.HttpProperties);
        }

        public void Dispose()
        {
            
        }
    }
}
