using KissLog.Internal;
using KissLog.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
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

            WebProperties webProperties = (WebProperties)ctx.Items[Constants.HttpRequestPropertiesKey];
            if (webProperties == null)
                return;

            webProperties.Request.IsNewSession = ctx.Session.IsNewSession;
            webProperties.Request.SessionId = ctx.Session.SessionID;
        }

        private void PostAuthenticateRquest(object sender, EventArgs e)
        {
            HttpContext ctx = HttpContext.Current;

            WebProperties webProperties = (WebProperties)ctx.Items[Constants.HttpRequestPropertiesKey];
            if(webProperties == null)
                return;

            if((ctx.User is ClaimsPrincipal) == false)
                return;

            ClaimsPrincipal claimsPrincipal = (ClaimsPrincipal)ctx.User;
            if(claimsPrincipal.Identity == null)
                return;

            if(claimsPrincipal.Identity.IsAuthenticated == false)
                return;

            webProperties.Request.IsAuthenticated = true;
            webProperties.Request.User = new UserDetails
            {
                Name = claimsPrincipal.Identity.Name,
            };

            ClaimsIdentity claimsIdentity = claimsPrincipal?.Identity as ClaimsIdentity;
            if(claimsIdentity == null)
                return;

            List<KeyValuePair<string, string>> claims = DataParser.ToDictionary(claimsIdentity);
            webProperties.Request.Properties.Claims = claims;

            UserDetails user = KissLogConfiguration.Options.ApplyGetUser(webProperties.Request.Properties);
            if (user != null)
            {
                user.Name = user.Name ?? claimsIdentity.Name;
            }

            webProperties.Request.User = user;
        }

        private void BeginRequest(object sender, EventArgs e)
        {
            HttpContext ctx = HttpContext.Current;
            var request = ctx.Request;

            KissLog.Web.WebProperties webProperties = new KissLog.Web.WebProperties
            {
                Request = HttpRequestFactory.Create(request)
            };

            ctx.Items[Constants.HttpRequestPropertiesKey] = webProperties;

            Logger logger = Logger.Factory.Get() as Logger;
            if(logger == null)
                return;

            logger.DataContainer.WebProperties = webProperties;

            KissLog.Internal.NotifyListeners.NotifyBeginRequest(webProperties.Request, logger);
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
                    string responseContent = null;

                    try
                    {
                        responseContent = sniffer.GetContent();
                    }
                    catch(Exception ex1)
                    {
                        InternalHelpers.Log(ex1.ToString(), LogLevel.Error);
                    }

                    if (string.IsNullOrEmpty(responseContent) == false)
                    {
                        logger.Log(LogLevel.Error, responseContent);
                    }
                }
            }

            WebProperties webProperties = (WebProperties)HttpContext.Current.Items[Constants.HttpRequestPropertiesKey];
            if(webProperties == null)
            {
                // IIS redirect bypasses the BeginRequest() event
                webProperties = new WebProperties
                {
                    Request = HttpRequestFactory.Create(ctx.Request)
                };
            }

            KissLog.Web.HttpResponse response = HttpResponseFactory.Create(ctx.Response);
            webProperties.Response = response;

            if (logger.DataContainer.ExplicitHttpStatusCode.HasValue)
            {
                response.HttpStatusCode = logger.DataContainer.ExplicitHttpStatusCode.Value;
            }

            if (sniffer != null)
            {
                response.Properties.ContentLength = ReadResponseLength(sniffer);

                LogResponse(logger, response, sniffer);
            }

            logger.DataContainer.WebProperties = webProperties;

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

        private void LogResponse(Logger logger, KissLog.Web.HttpResponse response, ResponseSniffer sniffer)
        {
            try
            {
                using (sniffer.MirrorStream)
                {
                    sniffer.MirrorStream.Position = 0;

                    if (InternalHelpers.PreFilterShouldLogResponseBody(logger, sniffer.MirrorStream, response.Properties))
                    {
                        using (TemporaryFile tempFile = new TemporaryFile())
                        {
                            using (var fs = File.OpenWrite(tempFile.FileName))
                            {
                                sniffer.MirrorStream.CopyTo(fs);
                            }

                            string responseFileName = InternalHelpers.ResponseFileName(response.Properties.Headers);
                            logger.LogFile(tempFile.FileName, responseFileName);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Error logging HTTP Response Content as file");
                sb.AppendLine(ex.ToString());

                KissLog.Internal.InternalHelpers.Log(sb.ToString(), LogLevel.Error);
            }
        }

        private long ReadResponseLength(ResponseSniffer sniffer)
        {
            if (sniffer != null)
            {
                try
                {
                    return sniffer.MirrorStream.Length;
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Error logging HTTP Response length");
                    sb.AppendLine(ex.ToString());

                    KissLog.Internal.InternalHelpers.Log(sb.ToString(), LogLevel.Error);

                    return 0;
                }
            }

            return 0;
        }
    }
}

