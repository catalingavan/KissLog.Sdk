using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using KissLog.Web;
using System.Security.Claims;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;

namespace KissLog.AspNetCore
{
    internal static class WebRequestPropertiesFactory
    {
        private static readonly int MaxKeyLength = 100;
        private static readonly int MaxValueLength = 1000;
        private static readonly int MaxInputStreamLength = 2000;

        public static WebRequestProperties Create(HttpRequest request)
        {
            WebRequestProperties result = new WebRequestProperties();

            if (request == null)
                return result;

            try
            {
                if (request.HttpContext.Session != null && request.HttpContext.Session.IsAvailable)
                {
                    bool isNewSession = false;

                    string lastSessionId = request.HttpContext.Session.GetString("X-KissLogSessionId");
                    if (string.IsNullOrEmpty(lastSessionId) || string.Compare(lastSessionId, request.HttpContext.Session.Id, true) != 0)
                    {
                        isNewSession = true;
                        request.HttpContext.Session.SetString("X-KissLogSessionId", request.HttpContext.Session.Id);
                    }

                    result.IsNewSession = isNewSession;
                    result.SessionId = request.HttpContext.Session.Id;
                }
            }
            catch { }

            result.StartDateTime = DateTime.UtcNow;
            result.UserAgent = request.Headers[HeaderNames.UserAgent].ToString();

            string url = request.GetDisplayUrl();
            result.Url = new Uri(url);

            result.MachineName = GetMachineName();

            RequestProperties requestProperties = new RequestProperties();
            result.Request = requestProperties;

            AddUserClaims(request, result);

            result.RemoteAddress = request.HttpContext.Connection?.RemoteIpAddress?.ToString();
            result.HttpMethod = request.Method;

            foreach (string key in request.Headers.Keys)
            {
                if(string.Compare(key, "Cookie", StringComparison.OrdinalIgnoreCase) == 0)
                    continue;

                StringValues values;
                request.Headers.TryGetValue(key, out values);

                string value = values.ToString();

                requestProperties.Headers.Add(TruncateValue(key, value));

                if (string.Compare(key, "Referer", true) == 0)
                    result.HttpReferer = value;
            }

            foreach (string key in request.Cookies.Keys)
            {
                if (KissLogConfiguration.ShouldReadCookie(key) == false)
                    continue;

                string value = request.Cookies[key];

                requestProperties.Cookies.Add(TruncateValue(key, value));
            }

            foreach (string key in request.Query.Keys)
            {
                string value = string.Join("; ", request.Query[key]);

                requestProperties.QueryString.Add(
                    new KeyValuePair<string, string>(key, value)
                );
            }

            if (request.HasFormContentType)
            {
                foreach (string key in request.Form.Keys)
                {
                    string value = string.Join("; ", request.Form[key]);

                    requestProperties.FormData.Add(TruncateValue(key, value));
                }
            }

            if (KissLogConfiguration.ShouldReadInputStream(result))
            {
                string inputStream = ReadInputStream(request);
                if (string.IsNullOrEmpty(inputStream) == false)
                {
                    requestProperties.InputStream = TruncateInputStream(inputStream);
                }
            }

            return result;
        }

        private static void AddUserClaims(HttpRequest request, WebRequestProperties properties)
        {
            if (request.HttpContext.User?.Identity == null || request.HttpContext.User.Identity.IsAuthenticated == false)
                return;

            if ((request.HttpContext.User != null) == false)
                return;

            ClaimsPrincipal claimsPrincipal = (ClaimsPrincipal)request.HttpContext.User;
            ClaimsIdentity identity = (ClaimsIdentity)claimsPrincipal?.Identity;

            if (identity == null)
                return;

            List<KeyValuePair<string, string>> claims = ToDictionary(identity);

            string userName = KissLogConfiguration.GetLoggedInUserName(claims);
            string emailAddress = KissLogConfiguration.GetLoggedInUserEmailAddress(claims);
            string avatar = KissLogConfiguration.GetLoggedInUserAvatar(claims);

            properties.Request.Claims = claims;

            properties.IsAuthenticated = true;
            properties.User = new UserDetails
            {
                Name = userName,
                EmailAddress = emailAddress,
                Avatar = avatar
            };
        }

        private static string GetMachineName()
        {
            string name = null;

            try
            {
                name =
                    Environment.GetEnvironmentVariable("CUMPUTERNAME") ??
                    Environment.GetEnvironmentVariable("HOSTNAME") ??
                    System.Net.Dns.GetHostName();
            }
            catch
            { }

            return name;
        }

        private static string ReadInputStream(HttpRequest request)
        {
            string content = string.Empty;

            try
            {
                if (request.Body.CanRead == false)
                    return content;

                // Allows using several time the stream in ASP.Net Core
                request.EnableRewind();

                // Arguments: Stream, Encoding, detect encoding, buffer size 
                // AND, the most important: keep stream opened
                using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
                {
                    content = reader.ReadToEnd();
                }

                request.Body.Position = 0;
            }
            catch
            {

            }

            return content;
        }

        public static List<KeyValuePair<string, string>> ToDictionary(ClaimsIdentity identity)
        {
            List<KeyValuePair<string, string>> claims =
                identity.Claims
                    .Where(p => string.IsNullOrEmpty(p.Type) == false)
                    .Select(p => new KeyValuePair<string, string>(p.Type, p.Value))
                    .ToList();

            return claims;
        }

        private static KeyValuePair<string, string> TruncateValue(string key, string value)
        {
            if (!string.IsNullOrEmpty(key) && key.Length > MaxKeyLength)
            {
                key = $"{key.Substring(0, MaxKeyLength - 3)}***";
            }

            if (!string.IsNullOrEmpty(value) && value.Length > MaxValueLength)
            {
                value = $"{value.Substring(0, MaxValueLength - 3)}***";
            }

            return new KeyValuePair<string, string>(key, value);
        }

        private static string TruncateInputStream(string inputStream)
        {
            if (string.IsNullOrEmpty(inputStream) || inputStream.Length <= MaxInputStreamLength)
                return inputStream;

            if (inputStream.Trim().StartsWith("{") == false)
                return $"{inputStream.Substring(0, MaxInputStreamLength - 3)}***";

            try
            {
                Dictionary<string, object> asDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(inputStream);
                if (asDictionary == null || !asDictionary.Any())
                    return $"{inputStream.Substring(0, MaxInputStreamLength - 3)}***";

                Dictionary<string, object> result = new Dictionary<string, object>();

                foreach (var item in asDictionary)
                {
                    string key = item.Key;
                    string value = item.Value == null ? null : item.Value.ToString();
                    bool valueChanged = false;

                    if (!string.IsNullOrEmpty(key) && key.Length > MaxKeyLength)
                    {
                        key = $"{key.Substring(0, MaxKeyLength - 3)}***";
                    }

                    if (!string.IsNullOrEmpty(value) && value.Length > MaxValueLength)
                    {
                        value = $"{value.Substring(0, MaxValueLength - 3)}***";
                        valueChanged = true;
                    }

                    result.Add(key, valueChanged ? value : item.Value);
                }

                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }
            catch
            {
                return $"{inputStream.Substring(0, MaxInputStreamLength - 3)}***";
            }
        }
    }
}
