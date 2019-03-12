using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog
{
    public class LogListenerParser2
    {
        public List<string> NoLogUrls = new List<string>();

        public List<string> NoLogResponseContentTypes = new List<string>
        {
            "application/octet-stream",
            "application/javascript",
            "text/javascript",
            "text/css",
            "text/event-stream",
            "application/ogg",
            "image/",
            "audio/",
            "video/"
        };

        public virtual bool ShouldLog(FlushLogArgs args, ILogListener logListener)
        {
            if (args.IsCreatedByHttpRequest == false)
                return true;

            if (args.WebRequestProperties?.Response == null)
                return true;

            int httpStatusCode = (int)args.WebRequestProperties.Response.HttpStatusCode;
            if (logListener.MinimumResponseHttpStatusCode > 0)
            {
                if (httpStatusCode < logListener.MinimumResponseHttpStatusCode)
                    return false;
            }

            string contentType = args.WebRequestProperties.Response.Headers.FirstOrDefault(p => string.Compare(p.Key, "content-type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            if (string.IsNullOrEmpty(contentType) == false)
            {
                if (NoLogResponseContentTypes?.Any() == true)
                {
                    if (NoLogResponseContentTypes.Any(p => contentType.Contains(p.ToLowerInvariant())))
                    {
                        return false;
                    }
                }
            }

            string localPath = args.WebRequestProperties.Url?.LocalPath.ToLowerInvariant();
            if (string.IsNullOrEmpty(localPath) == false)
            {
                if (NoLogUrls?.Any() == true)
                {
                    if (NoLogUrls.Any(p => localPath.Contains(p.ToLowerInvariant())))
                        return false;
                }
            }

            return true;
        }

        public virtual bool ShouldLog(LogMessage logMessage, ILogListener logListener)
        {
            if (logMessage == null)
                return false;

            if (logMessage.LogLevel < logListener.MinimumLogMessageLevel)
                return false;

            return true;
        }
    }
}
