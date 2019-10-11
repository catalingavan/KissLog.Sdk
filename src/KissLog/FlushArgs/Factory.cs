using KissLog.Web;
using System;

namespace KissLog.FlushArgs
{
    internal static class Factory
    {
        public static BeginRequestArgs CreateBeginRequestArgs(WebRequestProperties properties)
        {
            return new BeginRequestArgs
            {
                Url = properties.Url,
                UserAgent = properties.UserAgent,
                HttpMethod = properties.HttpMethod,
                RemoteAddress = properties.RemoteAddress,
                HttpReferer = properties.HttpReferer,
                IsNewSession = properties.IsNewSession,
                MachineName = properties.MachineName,
                IsAuthenticated = properties.IsAuthenticated,
                User = properties.User,
                StartDateTime = properties.StartDateTime,
                Request = properties.Request
            };
        }

        public static EndRequestArgs CreateEndRequestArgs(WebRequestProperties properties)
        {
            BeginRequestArgs beginRequestArgs = CreateBeginRequestArgs(properties);
            EndRequestArgs endRequestArgs = (EndRequestArgs)beginRequestArgs;

            DateTime endDateTime = properties.EndDateTime ?? DateTime.UtcNow;
            double durration = (endDateTime - properties.StartDateTime).TotalMilliseconds;

            endRequestArgs.Response = properties.Response;
            endRequestArgs.EndDateTime = endDateTime;
            endRequestArgs.DurationInMilliseconds = durration;

            return endRequestArgs;
        }
    }
}
