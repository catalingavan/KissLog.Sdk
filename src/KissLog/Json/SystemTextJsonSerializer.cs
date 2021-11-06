using System.Collections.Generic;
using System.Text.Json;

namespace KissLog.Json
{
    internal class SystemTextJsonSerializer : IJsonSerializer
    {
        public string Serialize(object json, JsonSerializeOptions options = null)
        {
            var jsonSerializerOptions = CreateJsonSerializerOptions(options);

            return JsonSerializer.Serialize(json, jsonSerializerOptions);
        }

        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        public IEnumerable<KeyValuePair<string, object>> DeserializeAndFlatten(string json)
        {
            if (IsJson(json, out JsonDocument document) == false)
                return new List<KeyValuePair<string, object>>();

            var service = new SystemTextJsonDeserializeAndFlatten();
            return service.DeserializeAndFlatten(document);
        }

        private JsonSerializerOptions CreateJsonSerializerOptions(JsonSerializeOptions options)
        {
            if (options == null)
                return null;

            return new JsonSerializerOptions
            {
                WriteIndented = options.WriteIndented
            };
        }

        public bool IsJson(string strInput)
        {
            return IsJson(strInput, out JsonDocument _);
        }

        private bool IsJson(string strInput, out JsonDocument document)
        {
            document = null;

            if (string.IsNullOrWhiteSpace(strInput))
                return false;

            strInput = strInput.Trim();

            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) ||
                (strInput.StartsWith("[") && strInput.EndsWith("]")))
            {
                try
                {
                    document = JsonDocument.Parse(strInput);
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
    }
}
