using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

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

        /// <summary>
        /// Function which filters / returns the messages to log, which are initially groupped by CategoryName
        /// </summary>
        public Func<IEnumerable<LogMessagesGroup>, IEnumerable<LogMessage>> MergeLogMessages = (logMessagesGroups) =>
        {
            if (logMessagesGroups == null || logMessagesGroups.Any() == false)
                return Enumerable.Empty<LogMessage>();

            if (logMessagesGroups.Count() == 1)
                return logMessagesGroups.First().Messages;

            return logMessagesGroups.SelectMany(p => p.Messages).OrderBy(p => p.DateTime).ToList();
        };

        public virtual bool ShouldLog(KissLog.Web.WebRequestProperties webRequestProperties, ILogListener logListener)
        {
            if (webRequestProperties == null)
                return false;

            if (logListener.MinimumResponseHttpStatusCode > 0)
            {
                HttpStatusCode httpStatusCode = webRequestProperties.Response?.HttpStatusCode ?? HttpStatusCode.OK;
                if ((int) httpStatusCode < logListener.MinimumResponseHttpStatusCode)
                    return false;
            }

            if (ContentTypesToIgnore != null && ContentTypesToIgnore.Any())
            {
                if (webRequestProperties.Response != null)
                {
                    var contentType = webRequestProperties.Response.Headers.FirstOrDefault(p => p.Key.ToLowerInvariant() == "content-type");
                    if (string.IsNullOrEmpty(contentType.Value) == false)
                    {
                        if (ContentTypesToIgnore.Any(p => contentType.Value.Contains(p.ToLowerInvariant())))
                        {
                            return false;
                        }
                    }
                }
            }

            if (UrlsToIgnore != null && UrlsToIgnore.Any())
            {
                string localPath = webRequestProperties.Url?.LocalPath.ToLowerInvariant();
                if (string.IsNullOrEmpty(localPath) == false)
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

        public virtual void RemoveDataBeforePersisting(FlushLogArgs args)
        {
            if (args.WebRequestProperties?.Request != null)
            {
                ObfuscateDictionary(args.WebRequestProperties.Request.Claims);
                ObfuscateDictionary(args.WebRequestProperties.Request.FormData);
                ObfuscateDictionary(args.WebRequestProperties.Request.QueryString);

                ObfuscateInputStream(args.WebRequestProperties.Request);
            }
        }

        public void ObfuscateDictionary(List<KeyValuePair<string, string>> dictionary)
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
                        dictionary.Insert(i, new KeyValuePair<string, string>(item.Key, KissLogConfiguration.ObfuscatedValue));
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
                            asDictionary[key] = KissLogConfiguration.ObfuscatedValue;
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
    }
}
