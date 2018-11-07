using KissLog.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;

namespace KissLog.AspNet.Web
{
    public class KissLogHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
            context.EndRequest += EndRequest;
            context.Error += OnError;
            context.PreRequestHandlerExecute += Context_PreRequestHandlerExecute;
            context.PostAuthenticateRequest += PostAuthenticateRquest;
            context.PostAcquireRequestState += Context_PostAcquireRequestState;
        }

        private void Context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            var application = sender as HttpApplication;
            if(application == null)
                return;

            var context = application.Context;
            var response = context.Response;

            // Add a filter to capture response stream
            response.Filter = new ResponseSniffer(response.Filter, response.ContentEncoding);
        }

        private void Context_PostAcquireRequestState(object sender, EventArgs e)
        {
            HttpContext ctx = HttpContext.Current;

            if (ctx.Session == null)
                return;

            // We need to add a dummy session value, otherwise ctx.Session.IsNewSession will always be true
            ctx.Session.Add("X-KissLogSession", true);

            WebRequestProperties requestProperties = (WebRequestProperties)ctx.Items[Constants.HttpRequestPropertiesKey];
            if (requestProperties == null)
                return;

            requestProperties.IsNewSession = ctx.Session.IsNewSession;
            requestProperties.SessionId = ctx.Session.SessionID;
        }

        private void PostAuthenticateRquest(object sender, EventArgs e)
        {
            HttpContext ctx = HttpContext.Current;

            WebRequestProperties requestProperties = (WebRequestProperties)ctx.Items[Constants.HttpRequestPropertiesKey];
            if(requestProperties == null)
                return;

            if((ctx.User is ClaimsPrincipal) == false)
                return;

            ClaimsPrincipal claimsPrincipal = (ClaimsPrincipal)ctx.User;
            if(claimsPrincipal.Identity == null)
                return;

            if(claimsPrincipal.Identity.IsAuthenticated == false)
                return;

            requestProperties.IsAuthenticated = true;
            requestProperties.User = new UserDetails
            {
                Name = claimsPrincipal.Identity.Name,
            };

            ClaimsIdentity claimsIdentity = claimsPrincipal?.Identity as ClaimsIdentity;

            if (claimsIdentity != null)
            {
                List<KeyValuePair<string, string>> claims = DataParser.ToDictionary(claimsIdentity);
                requestProperties.Request.Claims = claims;

                string userName = KissLogConfiguration.GetLoggedInUserName(requestProperties.Request);
                string emailAddress = KissLogConfiguration.GetLoggedInUserEmailAddress(requestProperties.Request);
                string avatar = KissLogConfiguration.GetLoggedInUserAvatar(requestProperties.Request);

                requestProperties.User = new UserDetails
                {
                    Name = userName ?? claimsIdentity.Name,
                    EmailAddress = emailAddress,
                    Avatar = avatar
                };
            }
        }

        private void BeginRequest(object sender, EventArgs e)
        {
            HttpContext ctx = HttpContext.Current;
            var request = ctx.Request;

            ILogger logger = LoggerFactory.GetInstance(ctx);
            (logger as Logger)?.AddProperty(InternalHelpers.IsCreatedByHttpRequest, true);

            WebRequestProperties requestProperties = WebRequestPropertiesFactory.Create(logger, request);
            ctx.Items[Constants.HttpRequestPropertiesKey] = requestProperties;
        }

        private void OnError(object sender, EventArgs eventArgs)
        {
            HttpContext ctx = HttpContext.Current;

            ILogger logger = LoggerFactory.GetInstance(ctx);

            if (logger == null)
                return;

            Exception ex = ctx.Server.GetLastError();
            if (ex != null)
            {
                logger.Log(LogLevel.Error, ex);
            }
        }

        private void EndRequest(object sender, EventArgs e)
        {
            HttpContext ctx = HttpContext.Current;

            ILogger logger = LoggerFactory.GetInstance(ctx);
            if (logger == null)
                return;

            Exception ex = ctx.Server.GetLastError();
            if (ex != null)
            {
                logger.Log(LogLevel.Error, ex);
            }
            
            var sniffer = ctx.Response.Filter as ResponseSniffer;

            if (ctx.Response.StatusCode >= 400 && ex == null)
            {
                if (sniffer != null)
                {
                    string response = sniffer.GetContent();
                    if (string.IsNullOrEmpty(response) == false)
                    {
                        logger.Log(LogLevel.Error, response);
                    }
                }
            }

            WebRequestProperties webRequestProperties = (WebRequestProperties)HttpContext.Current.Items[Constants.HttpRequestPropertiesKey];
            webRequestProperties.EndDateTime = DateTime.UtcNow;

            ResponseProperties responseProperties = new ResponseProperties();
            responseProperties.HttpStatusCode = (HttpStatusCode)ctx.Response.StatusCode;
            responseProperties.Headers = DataParser.ToDictionary(ctx.Response.Headers);

            webRequestProperties.Response = responseProperties;

            if (sniffer != null && ShouldLogResponseBody(logger, webRequestProperties))
            {
                string responseFileName = InternalHelpers.ResponseFileName(webRequestProperties.Response.Headers);

                using (sniffer.MirrorStream)
                {
                    sniffer.MirrorStream.Position = 0;

                    using (TemporaryFile tempFile = new TemporaryFile())
                    {
                        using (var fs = File.OpenWrite(tempFile.FileName))
                        {
                            sniffer.MirrorStream.CopyTo(fs);
                        }

                        logger.LogFile(tempFile.FileName, responseFileName);
                    }
                }
            }

            ((Logger) logger).SetWebRequestProperties(webRequestProperties);

            IEnumerable<ILogger> loggers = LoggerFactory.GetAll(ctx);

            Logger.NotifyListeners(loggers.ToArray());
        }

        public void Dispose()
        {

        }

        private bool ShouldLogResponseBody(ILogger logger, WebRequestProperties webRequestProperties)
        {
            if (logger is Logger theLogger)
            {
                var logResponse = theLogger.GetProperty(InternalHelpers.LogResponseBodyProperty);
                if (logResponse != null && logResponse is bool asBoolean)
                {
                    return asBoolean;
                }
            }

            return KissLogConfiguration.ShouldLogResponseBody(webRequestProperties);
        }
    }
}

