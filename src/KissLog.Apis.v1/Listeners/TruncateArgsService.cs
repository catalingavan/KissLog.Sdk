using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.Apis.v1.Listeners
{
    public class TruncateArgsService
    {
        private const int KeyLength = 100;
        private const int ValueLength = 1000;
        private const int MessageLength = 10000;

        private bool _jsonTrucated = false;

        public void Truncate(FlushLogArgs args)
        {
            if (args.WebRequestProperties?.Request != null)
            {
                Truncate(args.WebRequestProperties.Request.Headers);
                Truncate(args.WebRequestProperties.Request.Cookies);
                Truncate(args.WebRequestProperties.Request.QueryString);
                Truncate(args.WebRequestProperties.Request.FormData);
                Truncate(args.WebRequestProperties.Request.ServerVariables);
                Truncate(args.WebRequestProperties.Request.Claims);

                if (!string.IsNullOrEmpty(args.WebRequestProperties.Request.InputStream))
                {
                    string inputStream = TruncateJson(args.WebRequestProperties.Request.InputStream);
                    args.WebRequestProperties.Request.InputStream = inputStream;
                }
            }

            if (args.WebRequestProperties?.Response != null)
            {
                Truncate(args.WebRequestProperties.Response.Headers);
            }

            if (args.MessagesGroups != null)
            {
                foreach (var group in args.MessagesGroups)
                {
                    if (group.Messages != null)
                    {
                        foreach (var message in group.Messages)
                        {
                            Truncate(message);
                        }
                    }
                }
            }
        }

        private void Truncate(List<KeyValuePair<string, string>> dictionary)
        {
            if (dictionary == null)
                return;

            var tempDictionary = dictionary.ToList();

            for (int i = 0; i < tempDictionary.Count; i++)
            {
                var item = tempDictionary.ElementAt(i);

                string key = item.Key;
                string value = item.Value;

                if (!string.IsNullOrEmpty(key) && key.Length > KeyLength)
                {
                    key = $"{key.Substring(0, KeyLength)}***";
                }

                if (!string.IsNullOrEmpty(value) && value.Length > ValueLength)
                {
                    value = $"{value.Substring(0, ValueLength)}***";
                }

                dictionary.RemoveAt(i);
                dictionary.Insert(i, new KeyValuePair<string, string>(key, value));
            }
        }

        private void Truncate(LogMessage message)
        {
            if (!string.IsNullOrEmpty(message.Message) && message.Message.Length > MessageLength)
            {
                message.Message = $"{message.Message.Substring(0, MessageLength)}***";
            }
        }

        private string TruncateJson(string json)
        {
            _jsonTrucated = false;
            string resultJson = null;

            try
            {
                JToken token = JToken.Parse(json);
                Truncate(token);

                if (_jsonTrucated == true)
                {
                    resultJson = token.ToString();
                }
            }
            catch
            {
                // ignored
            }

            if (string.IsNullOrEmpty(resultJson))
                resultJson = json;

            return resultJson;
        }

        private void Truncate(JToken token)
        {
            if(token == null)
                return;

            switch (token.Type)
            {
                case JTokenType.Object:
                    foreach (JProperty prop in token.Children<JProperty>())
                    {
                        Truncate(prop.Value);
                    }
                    break;

                case JTokenType.Array:
                    foreach (JToken value in token.Children())
                    {
                        Truncate(value);
                    }
                    break;

                default:
                    JValue tokenValue = (JValue) token;
                    string valueAsString = tokenValue.Value?.ToString();

                    if (!string.IsNullOrEmpty(valueAsString) && valueAsString.Length > ValueLength)
                    {
                        valueAsString = $"{valueAsString.Substring(0, ValueLength)}***";
                        tokenValue.Value = valueAsString;

                        _jsonTrucated = true;
                    }
                    break;
            }
        }
    }
}
