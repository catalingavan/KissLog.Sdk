using System.Collections.Generic;

namespace KissLog.Json
{
    internal interface IJsonSerializer
    {
        string Serialize(object json, JsonSerializeOptions options = null);
        T Deserialize<T>(string json);
        IEnumerable<KeyValuePair<string, object>> DeserializeAndFlatten(string json);
        bool IsJson(string strInput);
    }
}
