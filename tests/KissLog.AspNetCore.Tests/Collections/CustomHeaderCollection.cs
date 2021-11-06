using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;

namespace KissLog.AspNetCore.Tests.Collections
{
    public class CustomHeaderCollection : IHeaderDictionary
    {
        private readonly Dictionary<string, StringValues> _dictionary;

        public CustomHeaderCollection()
        {
            _dictionary = new Dictionary<string, StringValues>();
        }

        public CustomHeaderCollection(IDictionary<string, StringValues> dictionary)
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

        public long? ContentLength { get; set; }

        public ICollection<string> Keys => _dictionary.Keys;

        public ICollection<StringValues> Values => _dictionary.Values;

        public int Count => _dictionary.Count;

        public bool IsReadOnly { get; set; }

        public void Add(string key, StringValues value)
        {
            
        }

        public void Add(KeyValuePair<string, StringValues> item)
        {
            
        }

        public void Clear()
        {
            
        }

        public bool Contains(KeyValuePair<string, StringValues> item)
        {
            return false;
        }

        public bool ContainsKey(string key) => _dictionary.ContainsKey(key);

        public void CopyTo(KeyValuePair<string, StringValues>[] array, int arrayIndex)
        {
            
        }

        public IEnumerator<KeyValuePair<string, StringValues>> GetEnumerator() => _dictionary.GetEnumerator();

        public bool Remove(string key)
        {
            return false;
        }

        public bool Remove(KeyValuePair<string, StringValues> item)
        {
            return false;
        }

        public bool TryGetValue(string key, out StringValues value) => _dictionary.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();
    }
}
