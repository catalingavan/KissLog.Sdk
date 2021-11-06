using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;

namespace KissLog.AspNetCore.Tests.Collections
{
    public class CustomFormCollection : IFormCollection
    {
        private readonly Dictionary<string, StringValues> _dictionary;

        public CustomFormCollection()
        {
            _dictionary = new Dictionary<string, StringValues>();
        }

        public CustomFormCollection(IDictionary<string, StringValues> dictionary)
        {
            _dictionary = new Dictionary<string, StringValues>(dictionary);
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
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                if (value.Count == 0)
                {
                    _dictionary.Remove(key);
                }
                else
                {
                    _dictionary[key] = value;
                }
            }
        }

        public ICollection<string> Keys => _dictionary.Keys;
        public int Count => _dictionary.Count;
        public IFormFileCollection Files => null;
        public bool ContainsKey(string key) => _dictionary.ContainsKey(key);
        public IEnumerator<KeyValuePair<string, StringValues>> GetEnumerator() => _dictionary.GetEnumerator();
        public bool TryGetValue(string key, out StringValues value) => _dictionary.TryGetValue(key, out value);
        IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();
    }
}
