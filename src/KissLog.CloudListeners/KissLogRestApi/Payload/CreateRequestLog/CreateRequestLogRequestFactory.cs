using KissLog.FlushArgs;
using KissLog.Internal;
using KissLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog
{
    internal static class CreateRequestLogRequestFactory
    {
        public static CreateRequestLogRequest Create(FlushLogArgs args)
        {
            CreateRequestLogRequest result = new CreateRequestLogRequest();

            result.SdkName = InternalHelpers.SdkName;
            result.SdkVersion = InternalHelpers.SdkVersion;

            if (args.WebProperties == null)
                return result;

            DateTime startDateTime = args.WebProperties.Request.StartDateTime;
            DateTime endDateTime = args.WebProperties.Response.EndDateTime;

            result.StartDateTime = startDateTime;
            result.DurationInMilliseconds = (endDateTime - startDateTime).TotalMilliseconds;

            result.WebRequest = ToWebRequestProperties(args.WebProperties);

            result.MachineName = args.WebProperties.Request.MachineName;

            result.IsNewSession = args.WebProperties.Request.IsNewSession;
            result.SessionId = args.WebProperties.Request.SessionId;

            result.IsAuthenticated = args.WebProperties.Request.IsAuthenticated;
            result.User = ToUser(args.WebProperties.Request.User);

            IEnumerable<KissLog.LogMessage> logMessages = args.MessagesGroups.SelectMany(p => p.Messages).OrderBy(p => p.DateTime).ToList();
            result.LogMessages = logMessages.Select(p => ToLogMessage(p, startDateTime)).ToList();

            result.Exceptions = args.CapturedExceptions?.Select(p => ToCapturedException(p)).ToList();

            result.CustomProperties = args.CustomProperties?.ToList();

            return result;
        }

        private static KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog.Web.WebRequestProperties ToWebRequestProperties(WebProperties webProperties)
        {
            if (webProperties == null)
                return null;

            KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog.Url url = ToUrl(webProperties.Request.Url);
            KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog.Web.RequestProperties requestProperties = ToRequestProperties(webProperties.Request.Properties);
            KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog.Web.ResponseProperties responseProperties = ToResponseProperties(webProperties.Response);

            return new KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog.Web.WebRequestProperties
            {
                Url = url,
                UserAgent = webProperties.Request.UserAgent,
                HttpMethod = webProperties.Request.HttpMethod,
                HttpReferer = webProperties.Request.HttpReferer,
                RemoteAddress = webProperties.Request.RemoteAddress,
                Request = requestProperties,
                Response = responseProperties
            };
        }

        private static KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog.Web.RequestProperties ToRequestProperties(KissLog.Web.RequestProperties item)
        {
            if (item == null)
                return null;

            return new KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog.Web.RequestProperties
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

        private static KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog.Web.ResponseProperties ToResponseProperties(KissLog.Web.HttpResponse item)
        {
            if (item == null)
                return null;

            return new KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog.Web.ResponseProperties
            {
                HttpStatusCode = (int)item.HttpStatusCode,
                HttpStatusCodeText = item.HttpStatusCode.ToString(),
                Headers = item.Properties.Headers,
                ContentLength = item.Properties.ContentLength,
            };
        }

        private static KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog.LogMessage ToLogMessage(KissLog.LogMessage item, DateTime startRequestDateTime)
        {
            if (item == null)
                return null;

            return new KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog.LogMessage
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

        private static KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog.User ToUser(KissLog.Web.UserDetails item)
        {
            if (item == null)
                return null;

            return new KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog.User
            {
                Name = item.Name,
                EmailAddress = item.EmailAddress,
                Avatar = item.Avatar
            };
        }

        private static KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog.Url ToUrl(Uri uri)
        {
            if (uri == null)
                return null;

            return new KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog.Url
            {
                Path = uri.LocalPath,
                PathAndQuery = uri.PathAndQuery,
                AbsoluteUri = uri.AbsoluteUri
            };
        }

        private static KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog.CapturedException ToCapturedException(KissLog.CapturedException item)
        {
            if (item == null)
                return null;

            return new KissLog.CloudListeners.KissLogRestApi.Payload.CreateRequestLog.CapturedException
            {
                ExceptionType = item.ExceptionType,
                ExceptionMessage = item.ExceptionMessage
            };
        }
    }
}
