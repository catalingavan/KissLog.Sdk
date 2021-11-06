using System;
using System.Collections.Generic;
using System.Text.Json;

namespace KissLog.Json
{
    internal class SystemTextJsonDeserializeAndFlatten
    {
        public IEnumerable<KeyValuePair<string, object>> DeserializeAndFlatten(JsonDocument document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            List<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();

            FillDictionary(result, document.RootElement, null);

            return result;
        }

        private void FillDictionary(List<KeyValuePair<string, object>> dict, JsonElement element, string path)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (JsonProperty prop in element.EnumerateObject())
                    {
                        if (!string.IsNullOrWhiteSpace(prop.Name))
                        {
                            FillDictionary(dict, prop.Value, Join(path, "." + prop.Name));
                        }
                    }
                    break;

                case JsonValueKind.Array:
                    int index = 0;
                    foreach (JsonElement item in element.EnumerateArray())
                    {
                        FillDictionary(dict, item, Join(path, "[]"));
                        index++;
                    }
                    break;

                default:
                    object value = GetValue(element);
                    dict.Add(new KeyValuePair<string, object>(path, value));
                    break;
            }
        }

        private object GetValue(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Undefined:
                case JsonValueKind.Null:
                    return null;

                case JsonValueKind.True:
                case JsonValueKind.False:
                    return element.GetBoolean();

                case JsonValueKind.String:
                    return element.GetString();

                case JsonValueKind.Number:
                    return element.GetDouble();
            }

            return null;
        }

        private string Join(string prefix, string path)
        {
            return $"{prefix}{path}".Trim().Trim('.');
        }
    }
}
