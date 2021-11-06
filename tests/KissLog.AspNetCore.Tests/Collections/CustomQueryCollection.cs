using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections;
using System.Collections.Generic;

namespace KissLog.AspNetCore.Tests.Collections
{
    public class CustomQueryCollection : IQueryCollection
    {
        private readonly Dictionary<string, StringValues> _query;
        public CustomQueryCollection(Dictionary<string, StringValues> query)
        {
            _query = query;
        }

        public StringValues this[string key]
        {
            get
            {
                if (TryGetValue(key, out var value))
                {
                    return value;
                }

                return StringValues.Empty;
            }
        }

        public int Count => _query.Count;
        public ICollection<string> Keys => _query.Keys;
        public bool ContainsKey(string key) => _query.ContainsKey(key);
        public bool TryGetValue(string key, out StringValues value) => _query.TryGetValue(key, out value);
        public IEnumerator<KeyValuePair<string, StringValues>> GetEnumerator() => _query.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _query.GetEnumerator();
        }
    }
}
