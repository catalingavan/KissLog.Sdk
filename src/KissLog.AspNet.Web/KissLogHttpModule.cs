using KissLog.Internal;
using KissLog.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

            WebRequestProperties properties = (WebRequestProperties)ctx.Items[Constants.HttpRequestPropertiesKey];
            if(properties == null)
                return;

            if((ctx.User is ClaimsPrincipal) == false)
                return;

            ClaimsPrincipal claimsPrincipal = (ClaimsPrincipal)ctx.User;
            if(claimsPrincipal.Identity == null)
                return;

            if(claimsPrincipal.Identity.IsAuthenticated == false)
                return;

            properties.IsAuthenticated = true;
            properties.User = new UserDetails
            {
                Name = claimsPrincipal.Identity.Name,
            };

            ClaimsIdentity claimsIdentity = claimsPrincipal?.Identity as ClaimsIdentity;
            if(claimsIdentity == null)
                return;

            List<KeyValuePair<string, string>> claims = DataParser.ToDictionary(claimsIdentity);
            properties.Request.Claims = claims;

            UserDetails user = KissLogConfiguration.Options.ApplyGetUser(properties.Request);
            if (user != null)
            {
                user.Name = user.Name ?? claimsIdentity.Name;
            }

            properties.User = user;
        }

        private void BeginRequest(object sender, EventArgs e)
        {
            HttpContext ctx = HttpContext.Current;
            var request = ctx.Request;

            ctx.Items[Constants.HttpRequestPropertiesKey] = WebRequestPropertiesFactory.Create(request);

            Logger logger = Logger.Factory.Get() as Logger;
            if(logger == null)
                return;

            logger.DataContainer.AddProperty(InternalHelpers.IsCreatedByHttpRequest, true);
        }

        private void OnError(object sender, EventArgs eventArgs)
        {
            HttpContext ctx = HttpContext.Current;

            Logger logger = Logger.Factory.Get() as Logger;
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

            Logger logger = Logger.Factory.Get() as Logger;
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
                    string responseContent = sniffer.GetContent();
                    if (string.IsNullOrEmpty(responseContent) == false)
                    {
                        logger.Log(LogLevel.Error, responseContent);
                    }
                }
            }

            WebRequestProperties properties = (WebRequestProperties)HttpContext.Current.Items[Constants.HttpRequestPropertiesKey];
            properties.EndDateTime = DateTime.UtcNow;

            ResponseProperties response = WebResponsePropertiesFactory.Create(ctx.Response);
            properties.Response = response;

            if (logger.DataContainer.ExplicitHttpStatusCode.HasValue)
            {
                response.HttpStatusCode = logger.DataContainer.ExplicitHttpStatusCode.Value;
            }

            if (sniffer != null)
            {
                response.ContentLength = sniffer.MirrorStream.Length;
            }

            if (sniffer != null && InternalHelpers.ShouldLogResponseBody(logger, response))
            {
                string responseFileName = InternalHelpers.ResponseFileName(properties.Response.Headers);

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

            logger.DataContainer.WebRequestProperties = properties;

            IEnumerable<ILogger> loggers = Logger.Factory.GetAll();

            Logger.NotifyListeners(loggers.ToArray());
        }

        public void Dispose()
        {

        }
        
        static KissLogHttpModule()
        {
            SetFactory();
        }

        private static void SetFactory()
        {
            IKissLoggerFactory loggerFactory = new AspNetWebLoggerFactory();

            PropertyInfo factoryProperty = typeof(Logger).GetProperty("Factory");
            if (factoryProperty != null)
            {
                factoryProperty.SetValue(Logger.Factory, loggerFactory, null);
            }
        }
    }
}

