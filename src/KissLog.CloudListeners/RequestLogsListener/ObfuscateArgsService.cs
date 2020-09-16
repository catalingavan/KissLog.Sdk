using KissLog.FlushArgs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.CloudListeners.RequestLogsListener
{
    public class ObfuscateArgsService
    {
        private static readonly string Placeholder = "***obfuscated***";

        public List<string> ObfuscateKeys = new List<string>
        {
            "password"
        };

        public Func<string, string, bool> ShouldObfuscate = (string key, string value) => true;

        public void Obfuscate(FlushLogArgs args)
        {
            if (args.WebProperties.Request != null)
            {
                Obfuscate(args.WebProperties.Request.Properties.Headers);
                Obfuscate(args.WebProperties.Request.Properties.Cookies);
                Obfuscate(args.WebProperties.Request.Properties.QueryString);
                Obfuscate(args.WebProperties.Request.Properties.FormData);
                Obfuscate(args.WebProperties.Request.Properties.ServerVariables);
                Obfuscate(args.WebProperties.Request.Properties.Claims);
            }
        }

        private void Obfuscate(List<KeyValuePair<string, string>> dictionary)
        {
            if (dictionary == null)
                return;

            if (ObfuscateKeys == null || ObfuscateKeys.Any() == false)
                return;

            var tempDictionary = dictionary.ToList();

            for (int i = 0; i < tempDictionary.Count; i++)
            {
                var item = tempDictionary.ElementAt(i);
                string key = item.Key?.ToLowerInvariant();

                if (string.IsNullOrEmpty(key) == false)
                {
                    if (ObfuscateKeys.Any(p => key.Contains(p.ToLowerInvariant())))
                    {
                        if (ShouldObfuscate != null && ShouldObfuscate(item.Key, item.Value))
                        {
                            dictionary.RemoveAt(i);
                            dictionary.Insert(i, new KeyValuePair<string, string>(item.Key, Placeholder));
                        }
                    }
                }
            }
        }
    }
}
