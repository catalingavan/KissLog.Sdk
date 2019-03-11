using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using KissLog.Internal;

namespace KissLog
{
    public class LogListenerParser
    {
        public List<string> UrlsToIgnore = new List<string> {"browserLink"};

        /// <summary>
        /// Http Response content-type which we won't log at all.
        /// ie: css, js, images
        /// </summary>
        public List<string> ContentTypesToIgnore = new List<string> {"text/javascript", "text/css"};

        /// <summary>
        /// Keys to obfuscate from QueryString, FormData, Claims, and InputStream dictionary
        /// </summary>
        public List<string> KeysToObfuscate = new List<string> {"password"};

        public virtual bool ShouldLog(FlushLogArgs args, ILogListener logListener)
        {
            if (args.IsCreatedByHttpRequest == false)
                return true;

            if (args.WebRequestProperties?.Response == null)
                return true;

            int httpStatusCode = (int)args.WebRequestProperties.Response.HttpStatusCode;
            string responseContentType = args.WebRequestProperties.Response.Headers.FirstOrDefault(p => string.Compare(p.Key, "content-type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            string localPath = args.WebRequestProperties.Url?.LocalPath.ToLowerInvariant();

            if (logListener.MinimumResponseHttpStatusCode > 0)
            {
                if (httpStatusCode < logListener.MinimumResponseHttpStatusCode)
                    return false;
            }

            if (string.IsNullOrEmpty(responseContentType) == false)
            {
                if (ContentTypesToIgnore?.Any() == true)
                {
                    if (ContentTypesToIgnore.Any(p => responseContentType.Contains(p.ToLowerInvariant())))
                    {
                        return false;
                    }
                }
            }

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

        public virtual void AlterDataBeforePersisting(FlushLogArgs args)
        {
            if(args.WebRequestProperties?.Request != null)
            {
                ObfuscateDictionary(args.WebRequestProperties.Request.Claims);
                ObfuscateDictionary(args.WebRequestProperties.Request.FormData);
                ObfuscateDictionary(args.WebRequestProperties.Request.QueryString);
                ObfuscateInputStream(args.WebRequestProperties.Request);
            }
        }

        private void ObfuscateDictionary(List<KeyValuePair<string, string>> dictionary)
        {
            if (dictionary == null)
                return;

            if(KeysToObfuscate == null || KeysToObfuscate.Any() == false)
                return;

            var tempDictionary = dictionary.ToList();

            for (int i = 0; i < tempDictionary.Count; i++)
            {
                var item = tempDictionary.ElementAt(i);
                string key = item.Key?.ToLowerInvariant();

                if (string.IsNullOrEmpty(key) == false)
                {
                    if (KeysToObfuscate.Any(p => key.Contains(p.ToLowerInvariant())))
                    {
                        dictionary.RemoveAt(i);
                        dictionary.Insert(i, new KeyValuePair<string, string>(item.Key, InternalHelpers.ObfuscatedPlaceholder));
                    }
                }
            }
        }

        private void ObfuscateInputStream(KissLog.Web.RequestProperties requestProperties)
        {
            if(requestProperties == null)
                return;

            if (KeysToObfuscate == null || KeysToObfuscate.Any() == false)
                return;

            if (string.IsNullOrEmpty(requestProperties.InputStream) == true)
                return;

            if(requestProperties.InputStream.Trim().StartsWith("{") == false)
                return;

            try
            {
                Dictionary<string, object> asDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(requestProperties.InputStream);

                if (asDictionary != null)
                {
                    IEnumerable<string> keys =
                        asDictionary
                            .Where(p => string.IsNullOrEmpty(p.Key) == false)
                            .Select(p => p.Key).ToList();

                    bool updated = false;

                    foreach (string key in keys)
                    {
                        if (KeysToObfuscate.Any(p => key.ToLower().Contains(p)))
                        {
                            asDictionary[key] = InternalHelpers.ObfuscatedPlaceholder;
                            updated = true;
                        }
                    }

                    if (updated == true)
                    {
                        requestProperties.InputStream = JsonConvert.SerializeObject(asDictionary, Formatting.Indented);
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        [Obsolete("This method is deprecated. Use ShouldLog(FlushLogArgs args, ILogListener logListener) instead.")]
        public virtual bool ShouldLog(KissLog.Web.WebRequestProperties webRequestProperties, ILogListener logListener)
        {
            if (webRequestProperties?.Response == null)
                return true;

            int httpStatusCode = (int)webRequestProperties.Response.HttpStatusCode;
            string responseContentType = webRequestProperties.Response.Headers.FirstOrDefault(p => string.Compare(p.Key, "content-type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            string localPath = webRequestProperties.Url?.LocalPath.ToLowerInvariant();

            if (logListener.MinimumResponseHttpStatusCode > 0)
            {
                if (httpStatusCode < logListener.MinimumResponseHttpStatusCode)
                    return false;
            }

            if (string.IsNullOrEmpty(responseContentType) == false)
            {
                if (ContentTypesToIgnore?.Any() == true)
                {
                    if (ContentTypesToIgnore.Any(p => responseContentType.Contains(p.ToLowerInvariant())))
                    {
                        return false;
                    }
                }
            }

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

        [Obsolete("This method is deprecated. Use AlterDataBeforePersisting(FlushLogArgs args) instead.")]
        public virtual void RemoveDataBeforePersisting(FlushLogArgs args)
        {
            AlterDataBeforePersisting(args);
        }
    }
}
