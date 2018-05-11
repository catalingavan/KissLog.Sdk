using System;
using System.Collections.Generic;

namespace KissLog
{
    public class LogMessage
    {
        public string CategoryName { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public int Index { get; set; }

        public string MemberType { get; set; }
        public string MemberName { get; set; }
        public int LineNumber { get; set; }

        public Dictionary<string, object> CustomProperties { get; private set; }

        public LogMessage AddProp(string key, object value)
        {
            if (CustomProperties == null)
                CustomProperties = new Dictionary<string, object>();

            if (CustomProperties.ContainsKey(key))
            {
                CustomProperties[key] = value;
            }
            else
            {
                CustomProperties.Add(key, value);
            }

            return this;
        }

        public object GetProp(string key)
        {
            if (CustomProperties == null || !CustomProperties.ContainsKey(key))
                return null;

            return CustomProperties[key];
        }
    }
}

