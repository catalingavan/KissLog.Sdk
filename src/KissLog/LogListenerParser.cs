using KissLog.FlushArgs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog
{
    public class LogListenerParser
    {
        #region Obsolete >= 08-04-2019

        [Obsolete("AlterDataBeforePersisting is obsolete. Use BeforeFlush(FlushLogArgs args, ILogListener logListener) instead.", true)]
        public virtual void AlterDataBeforePersisting(FlushLogArgs args)
        {
            
        }

        #endregion

        public List<string> UrlsToIgnore = new List<string>();

        public List<string> ContentTypesToIgnore = new List<string>
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

        public virtual bool ShouldLog(BeginRequestArgs args)
        {
            if (args.Request == null)
                return true;

            string localPath = args.Request.Url.LocalPath.ToLowerInvariant();
            if (string.IsNullOrEmpty(localPath) == false)
            {
                if (UrlsToIgnore?.Any() == true)
                {
                    if (UrlsToIgnore.Any(p => localPath.Contains(p.ToLowerInvariant())))
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

        public virtual bool ShouldLog(FlushLogArgs args, ILogListener logListener)
        {
            if (args.IsCreatedByHttpRequest == false)
                return true;

            if (args.WebProperties.Response == null)
                return true;

            int httpStatusCode = (int)args.WebProperties.Response.HttpStatusCode;
            if (logListener.MinimumResponseHttpStatusCode > 0)
            {
                if (httpStatusCode < logListener.MinimumResponseHttpStatusCode)
                    return false;
            }

            string contentType = args.WebProperties.Response.Properties.Headers.FirstOrDefault(p => string.Compare(p.Key, "content-type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            if (string.IsNullOrEmpty(contentType) == false)
            {
                if (ContentTypesToIgnore?.Any() == true)
                {
                    if (ContentTypesToIgnore.Any(p => contentType.Contains(p.ToLowerInvariant())))
                    {
                        return false;
                    }
                }
            }

            string localPath = args.WebProperties.Request.Url?.LocalPath.ToLowerInvariant();
            if (string.IsNullOrEmpty(localPath) == false)
            {
                if (UrlsToIgnore?.Any() == true)
                {
                    if (UrlsToIgnore.Any(p => localPath.Contains(p.ToLowerInvariant())))
                        return false;
                }
            }

            return true;
        }

        public virtual void BeforeFlush(FlushLogArgs args, ILogListener logListener)
        {
            
        }
    }
}
