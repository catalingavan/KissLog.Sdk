using KissLog.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace KissLog.AspNetCore
{
    internal class HttpRequestFactory
    {
        public static KissLog.Http.HttpRequest Create(Microsoft.AspNetCore.Http.HttpRequest httpRequest)
        {
            if (httpRequest == null)
                throw new ArgumentNullException(nameof(httpRequest));

            Session session = KissLog.InternalHelpers.WrapInTryCatch(() => GetSession(httpRequest));
            session = session ?? new Session();

            bool isAuthenticated = httpRequest.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

            KissLog.Http.HttpRequest result = new KissLog.Http.HttpRequest(new KissLog.Http.HttpRequest.CreateOptions
            {
                Url = new Uri(GetDisplayUrl(httpRequest)),
                HttpMethod = httpRequest.Method,
                UserAgent = GetUserAgent(httpRequest.Headers),
                HttpReferer = GetHttpReferrer(httpRequest.Headers),
                RemoteAddress = httpRequest.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                MachineName = InternalHelpers.GetMachineName(),
                IsNewSession = session.IsNewSession,
                SessionId = session.SessionId,
                IsAuthenticated = isAuthenticated
            });

            RequestProperties.CreateOptions propertiesOptions = new RequestProperties.CreateOptions();
            propertiesOptions.Cookies = InternalHelpers.ToKeyValuePair(httpRequest.Cookies);
            propertiesOptions.Headers = InternalHelpers.ToKeyValuePair(httpRequest.Headers);
            propertiesOptions.QueryString = InternalHelpers.ToKeyValuePair(httpRequest.Query);
            propertiesOptions.Claims = GetClaims(httpRequest);

            if(httpRequest.HasFormContentType)
            {
                if (KissLogConfiguration.Options.Handlers.ShouldLogFormData.Invoke(result) == true)
                {
                    propertiesOptions.FormData = InternalHelpers.ToKeyValuePair(httpRequest.Form);
                }
            }

            if (KissLog.InternalHelpers.CanReadRequestInputStream(propertiesOptions.Headers))
            {
                if (KissLogConfiguration.Options.Handlers.ShouldLogInputStream.Invoke(result) == true)
                {
                    propertiesOptions.InputStream = KissLog.InternalHelpers.WrapInTryCatch(() =>
                    {
                        return ModuleInitializer.ReadInputStreamProvider.ReadInputStream(httpRequest);
                    });
                }
            }

            result.SetProperties(new RequestProperties(propertiesOptions));

            return result;
        }

        private static string GetUserAgent(IDictionary<string, StringValues> requestHeaders)
        {
            if (requestHeaders == null)
                return null;

            if(requestHeaders.ContainsKey(HeaderNames.UserAgent))
                return requestHeaders[HeaderNames.UserAgent].ToString();

            return null;
        }

        private static string GetHttpReferrer(IDictionary<string, StringValues> requestHeaders)
        {
            if (requestHeaders == null)
                return null;

            if (requestHeaders.ContainsKey(HeaderNames.Referer))
                return requestHeaders[HeaderNames.Referer].ToString();

            return null;
        }

        private static IEnumerable<KeyValuePair<string, string>> GetClaims(Microsoft.AspNetCore.Http.HttpRequest httpRequest)
        {
            if (httpRequest == null)
                throw new ArgumentNullException(nameof(httpRequest));

            IIdentity identity = httpRequest.HttpContext?.User?.Identity;
            if(identity == null)
                return new List<KeyValuePair<string, string>>();

            if (identity is ClaimsIdentity == false)
                return new List<KeyValuePair<string, string>>();

            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;

            List<KeyValuePair<string, string>> claims = InternalHelpers.ToKeyValuePair(claimsIdentity);

            return claims;
        }

        private static Session GetSession(Microsoft.AspNetCore.Http.HttpRequest httpRequest)
        {
            if (httpRequest == null)
                throw new ArgumentNullException(nameof(httpRequest));

            bool isNewSession = false;
            string sessionId = null;

            if (httpRequest.HttpContext?.Session != null && httpRequest.HttpContext.Session.IsAvailable == true)
            {
                if(httpRequest.HttpContext.Session.TryGetValue("X-KissLogSessionId", out var value) && value != null)
                {
                    sessionId = System.Text.Encoding.UTF8.GetString(value);
                }

                if(string.IsNullOrEmpty(sessionId) || string.Equals(sessionId, httpRequest.HttpContext.Session.Id, StringComparison.OrdinalIgnoreCase))
                {
                    isNewSession = true;
                    var sessionIdBytes = Encoding.UTF8.GetBytes(httpRequest.HttpContext.Session.Id);
                    httpRequest.HttpContext.Session.Set("X-KissLogSessionId", sessionIdBytes);
                }

                sessionId = httpRequest.HttpContext.Session.Id;
            }

            return new Session
            {
                IsNewSession = isNewSession,
                SessionId = sessionId
            };
        }

        private static string GetDisplayUrl(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            string value = request.Host.Value;
            string value2 = request.PathBase.Value ?? string.Empty;
            string value3 = request.Path.Value ?? string.Empty;
            string value4 = request.QueryString.Value ?? string.Empty;
            return new StringBuilder(request.Scheme.Length + "://".Length + value.Length + value2.Length + value3.Length + value4.Length).Append(request.Scheme).Append("://").Append(value)
                .Append(value2)
                .Append(value3)
                .Append(value4)
                .ToString();
        }

        class Session
        {
            public string SessionId { get; set; }
            public bool IsNewSession { get; set; }
        }
    }
}
