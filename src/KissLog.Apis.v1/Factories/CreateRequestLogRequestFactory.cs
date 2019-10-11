using KissLog.FlushArgs;
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

            if (args.EndRequestArgs == null)
                return result;

            DateTime startDateTime = args.EndRequestArgs.StartDateTime;
            DateTime endDateTime = args.EndRequestArgs.EndDateTime;

            result.StartDateTime = startDateTime;
            result.DurationInMilliseconds = (endDateTime - startDateTime).TotalMilliseconds;

            result.WebRequest = ToWebRequestProperties(args.BeginRequestArgs, args.EndRequestArgs);

            result.MachineName = args.EndRequestArgs.MachineName;

            result.IsNewSession = args.EndRequestArgs.IsNewSession;
            result.SessionId = args.EndRequestArgs.SessionId;

            result.IsAuthenticated = args.EndRequestArgs.IsAuthenticated;
            result.User = ToUser(args.EndRequestArgs.User);

            IEnumerable<LogMessage> logMessages = args.MessagesGroups.SelectMany(p => p.Messages).OrderBy(p => p.DateTime).ToList();
            result.LogMessages = logMessages.Select(p => ToLogMessage(p, startDateTime)).ToList();

            result.Exceptions = args.CapturedExceptions?.Select(p => ToCapturedException(p)).ToList();

            result.CustomProperties = args.CustomProperties?.ToList();

            return result;
        }

        private static Requests.Web.WebRequestProperties ToWebRequestProperties(BeginRequestArgs beginRequest, EndRequestArgs endRequest)
        {
            if (endRequest == null)
                return null;

            Requests.Url url = ToUrl(beginRequest.Url);
            Requests.Web.RequestProperties requestProperties = ToRequestProperties(beginRequest.Request);
            Requests.Web.ResponseProperties responseProperties = ToResponseProperties(endRequest.Response);

            return new Requests.Web.WebRequestProperties
            {
                Url = url,
                UserAgent = beginRequest.UserAgent,
                HttpMethod = beginRequest.HttpMethod,
                HttpReferer = beginRequest.HttpReferer,
                RemoteAddress = beginRequest.RemoteAddress,
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
