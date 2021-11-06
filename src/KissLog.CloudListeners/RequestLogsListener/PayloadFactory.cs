using KissLog.RestClient.Requests.CreateRequestLog;
using KissLog.RestClient.Requests.CreateRequestLog.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace KissLog.CloudListeners.RequestLogsListener
{
    internal static class PayloadFactory
    {
        public static CreateRequestLogRequest Create(FlushLogArgs args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            KissLogPackage kissLogPackage = KissLogConfiguration.KissLogPackages.GetPrimaryPackage();

            CreateRequestLogRequest result = new CreateRequestLogRequest();

            result.SdkName = kissLogPackage.Name;
            result.SdkVersion = kissLogPackage.Version.ToString();

            DateTime startDateTime = args.HttpProperties.Request.StartDateTime;
            DateTime endDateTime = args.HttpProperties.Response.EndDateTime;

            result.StartDateTime = startDateTime;
            result.DurationInMilliseconds = Math.Max(0, (endDateTime - startDateTime).TotalMilliseconds);

            result.WebRequest = Create(args.HttpProperties);

            result.MachineName = args.HttpProperties.Request.MachineName;

            result.IsNewSession = args.HttpProperties.Request.IsNewSession;
            result.SessionId = args.HttpProperties.Request.SessionId;

            result.IsAuthenticated = args.HttpProperties.Request.IsAuthenticated;
            result.User = CreateUser(args.HttpProperties.Request);

            IEnumerable<KissLog.LogMessage> logMessages = args.MessagesGroups.SelectMany(p => p.Messages).OrderBy(p => p.DateTime).ToList();
            result.LogMessages = logMessages.Select(p => Create(p, startDateTime)).ToList();

            result.Exceptions = args.Exceptions?.Select(p => Create(p)).ToList();

            result.CustomProperties = args.CustomProperties.ToList();

            return result;
        }

        internal static HttpProperties Create(KissLog.Http.HttpProperties httpProperties)
        {
            if (httpProperties == null)
                throw new ArgumentNullException(nameof(httpProperties));

            Url url = Create(httpProperties.Request.Url);
            RequestProperties requestProperties = Create(httpProperties.Request.Properties);
            ResponseProperties responseProperties = Create(httpProperties.Response);

            return new HttpProperties
            {
                Url = url,
                UserAgent = httpProperties.Request.UserAgent,
                HttpMethod = httpProperties.Request.HttpMethod,
                HttpReferer = httpProperties.Request.HttpReferer,
                RemoteAddress = httpProperties.Request.RemoteAddress,
                Request = requestProperties,
                Response = responseProperties
            };
        }

        internal static RequestProperties Create(KissLog.Http.RequestProperties requestProperties)
        {
            if (requestProperties == null)
                throw new ArgumentNullException(nameof(requestProperties));

            return new RequestProperties
            {
                Cookies = requestProperties.Cookies.ToList(),
                Headers = requestProperties.Headers.ToList(),
                Claims = requestProperties.Claims.ToList(),
                QueryString = requestProperties.QueryString.ToList(),
                FormData = requestProperties.FormData.ToList(),
                ServerVariables = requestProperties.ServerVariables.ToList(),
                InputStream = requestProperties.InputStream
            };
        }

        internal static ResponseProperties Create(KissLog.Http.HttpResponse httpResponse)
        {
            if (httpResponse == null)
                throw new ArgumentNullException(nameof(httpResponse));

            return new ResponseProperties
            {
                HttpStatusCode = httpResponse.StatusCode,
                HttpStatusCodeText = ((HttpStatusCode)httpResponse.StatusCode).ToString(),
                Headers = httpResponse.Properties.Headers.ToList(),
                ContentLength = httpResponse.Properties.ContentLength,
            };
        }

        internal static KissLog.RestClient.Requests.CreateRequestLog.LogMessage Create(LogMessage message, DateTime startRequestDateTime)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var result = new KissLog.RestClient.Requests.CreateRequestLog.LogMessage
            {
                CategoryName = message.CategoryName,
                LogLevel = message.LogLevel.ToString(),
                Message = message.Message,
                MillisecondsSinceStartRequest = Math.Max(0, (message.DateTime - startRequestDateTime).TotalMilliseconds),
                MemberType = message.MemberType,
                MemberName = message.MemberName,
                LineNumber = message.LineNumber
            };

            return result;
        }

        internal static Url Create(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return new Url
            {
                Path = uri.LocalPath,
                PathAndQuery = uri.PathAndQuery,
                AbsoluteUri = uri.AbsoluteUri
            };
        }

        internal static KissLog.RestClient.Requests.CreateRequestLog.CapturedException Create(CapturedException exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            return new KissLog.RestClient.Requests.CreateRequestLog.CapturedException
            {
                ExceptionType = exception.Type,
                ExceptionMessage = exception.Message
            };
        }

        internal static User CreateUser(KissLog.Http.HttpRequest httpRequest)
        {
            if (httpRequest == null)
                throw new ArgumentNullException(nameof(httpRequest));

            User user = RequestLogsApiListener.Options.Handlers.CreateUserPayload.Invoke(httpRequest);

            if (IsNull(user))
                return null;

            return user;
        }

        private static bool IsNull(User user)
        {
            if (user == null)
                return true;

            if (string.IsNullOrWhiteSpace(user.Name) && string.IsNullOrWhiteSpace(user.EmailAddress))
                return true;

            return false;
        }
    }
}
