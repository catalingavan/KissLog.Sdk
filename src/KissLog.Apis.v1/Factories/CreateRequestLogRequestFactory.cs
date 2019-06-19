using KissLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.Apis.v1.Factories
{
    internal static class CreateRequestLogRequestFactory
    {
        public static Requests.CreateRequestLogRequest Create(FlushLogArgs args)
        {
            Requests.CreateRequestLogRequest result = new Requests.CreateRequestLogRequest();

            result.SdkName = InternalHelpers.SdkName;
            result.SdkVersion = InternalHelpers.SdkVersion;

            if (args?.WebRequestProperties == null)
                return result;

            DateTime startDateTime = args.WebRequestProperties.StartDateTime;
            DateTime endDateTime = args.WebRequestProperties.EndDateTime ?? DateTime.UtcNow;

            result.StartDateTime = startDateTime;
            result.DurationInMilliseconds = (endDateTime - startDateTime).TotalMilliseconds;

            result.WebRequest = ToWebRequestProperties(args.WebRequestProperties);

            result.MachineName = args.WebRequestProperties.MachineName;

            result.IsNewSession = args.WebRequestProperties.IsNewSession;
            result.SessionId = args.WebRequestProperties.SessionId;

            result.IsAuthenticated = args.WebRequestProperties.IsAuthenticated;
            result.User = ToUser(args.WebRequestProperties.User);

            IEnumerable<LogMessage> logMessages = args.MessagesGroups.SelectMany(p => p.Messages).OrderBy(p => p.DateTime).ToList();
            result.LogMessages = logMessages.Select(p => ToLogMessage(p, startDateTime)).ToList();

            result.Exceptions = args.CapturedExceptions?.Select(p => ToCapturedException(p)).ToList();

            return result;
        }

        private static Requests.Web.WebRequestProperties ToWebRequestProperties(KissLog.Web.WebRequestProperties item)
        {
            if (item == null)
                return null;

            Requests.Url url = ToUrl(item.Url);
            Requests.Web.RequestProperties requestProperties = ToRequestProperties(item.Request);
            Requests.Web.ResponseProperties responseProperties = ToResponseProperties(item.Response);

            return new Requests.Web.WebRequestProperties
            {
                Url = url,
                UserAgent = item.UserAgent,
                HttpMethod = item.HttpMethod,
                HttpReferer = item.HttpReferer,
                RemoteAddress = item.RemoteAddress,
                Request = requestProperties,
                Response = responseProperties
            };
        }

        private static Requests.Web.RequestProperties ToRequestProperties(KissLog.Web.RequestProperties item)
        {
            if (item == null)
                return null;

            return new Requests.Web.RequestProperties
            {
                Cookies = item.Cookies,
                Headers = item.Headers,
                Claims = item.Claims,
                QueryString = item.QueryString,
                FormData = item.FormData,
                ServerVariables = item.ServerVariables,
                InputStream = item.InputStream
            };
        }

        private static Requests.Web.ResponseProperties ToResponseProperties(KissLog.Web.ResponseProperties item)
        {
            if (item == null)
                return null;

            return new Requests.Web.ResponseProperties
            {
                Headers = item.Headers,
                HttpStatusCode = (int)item.HttpStatusCode,
                HttpStatusCodeText = item.HttpStatusCode.ToString(),
                ContentLength = item.ContentLength
            };
        }

        private static Requests.LogMessage ToLogMessage(LogMessage item, DateTime startRequestDateTime)
        {
            if (item == null)
                return null;

            return new Requests.LogMessage
            {
                CategoryName = item.CategoryName,
                LogLevel = item.LogLevel.ToString(),
                Message = item.Message,
                MillisecondsSinceStartRequest = (item.DateTime - startRequestDateTime).TotalMilliseconds,
                MemberType = item.MemberType,
                MemberName = item.MemberName,
                LineNumber = item.LineNumber
            };
        }

        private static Requests.User ToUser(KissLog.Web.UserDetails item)
        {
            if (item == null)
                return null;

            return new Requests.User
            {
                Name = item.Name,
                EmailAddress = item.EmailAddress,
                Avatar = item.Avatar
            };
        }

        private static Requests.Url ToUrl(Uri uri)
        {
            if (uri == null)
                return null;

            return new Requests.Url
            {
                Path = uri.LocalPath,
                PathAndQuery = uri.PathAndQuery,
                AbsoluteUri = uri.AbsoluteUri
            };
        }

        private static Requests.CapturedException ToCapturedException(KissLog.CapturedException item)
        {
            if (item == null)
                return null;

            return new Requests.CapturedException
            {
                ExceptionType = item.ExceptionType,
                ExceptionMessage = item.ExceptionMessage
            };
        }
    }
}
