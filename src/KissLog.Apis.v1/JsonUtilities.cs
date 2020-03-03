using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace KissLog.Apis.v1
{
    internal static class JsonUtilities
    {
        public static bool IsJsonValid(string strInput, out JToken token)
        {
            token = null;

            if (string.IsNullOrEmpty(strInput))
                return false;

            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || // For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) // For array
            {
                try
                {
                    token = JToken.Parse(strInput);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static IList<KeyValuePair<string, object>> DeserializeAndFlatten(string json)
        {
            List<KeyValuePair<string, object>> dict = new List<KeyValuePair<string, object>>();

            if (IsJsonValid(json, out JToken token) == false)
                return dict;

            FillDictionaryFromJToken(dict, token, "");
            return dict;
        }

        private static void FillDictionaryFromJToken(List<KeyValuePair<string, object>> dict, JToken token, string prefix)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    foreach (JProperty prop in token.Children<JProperty>())
                    {
                        if (!string.IsNullOrEmpty(prop.Name))
                        {
                            FillDictionaryFromJToken(dict, prop.Value, Join(prefix, prop.Name));
                        }
                    }
                    break;

                case JTokenType.Array:
                    int index = 0;
                    foreach (JToken value in token.Children())
                    {
                        FillDictionaryFromJToken(dict, value, Join(prefix, "[]"));
                        index++;
                    }
                    break;

                default:
                    dict.Add(new KeyValuePair<string, object>(prefix, ((JValue)token).Value));
                    break;
            }
        }

        private static string Join(string prefix, string name)
        {
            return (string.IsNullOrEmpty(prefix) ? name : prefix + "." + name);
        }
    }
}
