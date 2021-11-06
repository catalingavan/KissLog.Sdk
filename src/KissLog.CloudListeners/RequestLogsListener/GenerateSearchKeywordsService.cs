using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace KissLog.CloudListeners.RequestLogsListener
{
    public class GenerateSearchKeywordsService
    {
        private static readonly Regex KeyRegex = new Regex(@"^[a-zA-Z0-9_-]*$", RegexOptions.Compiled);
        private static readonly Regex ValueRegex = new Regex(@"^[a-zA-Z0-9_\-\+\/.@: ]*$", RegexOptions.Compiled);

        private readonly Options _options;
        public GenerateSearchKeywordsService() : this(new Options())
        {

        }
        public GenerateSearchKeywordsService(Options options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public IEnumerable<string> GenerateKeywords(FlushLogArgs flushLogArgs)
        {
            if (flushLogArgs == null)
                throw new ArgumentNullException(nameof(flushLogArgs));

            List<string> result = new List<string>();

            result.AddRange(Get(flushLogArgs.HttpProperties.Request.Properties.QueryString));
            result.AddRange(Get(flushLogArgs.HttpProperties.Request.Properties.FormData));
            result.AddRange(GetFromInputStream(flushLogArgs.HttpProperties.Request.Properties.InputStream));
            result.AddRange(Get(flushLogArgs.Exceptions));

            return result;
        }

        private IEnumerable<string> Get(IEnumerable<KeyValuePair<string, string>> values)
        {
            List<string> result = new List<string>();

            foreach(var item in values)
            {
                if (string.IsNullOrWhiteSpace(item.Key) || item.Key.Length < _options.KeyCriteria.MinimumLength || item.Key.Length > _options.KeyCriteria.MaximumLength)
                    continue;

                if (string.IsNullOrWhiteSpace(item.Value) || item.Value.Length < _options.ValueCriteria.MinimumLength || item.Value.Length > _options.ValueCriteria.MaximumLength)
                    continue;

                if (_options.KeyCriteria.Pattern != null && _options.KeyCriteria.Pattern.IsMatch(item.Key) == false)
                    continue;

                if (_options.ValueCriteria.Pattern != null && _options.ValueCriteria.Pattern.IsMatch(item.Value) == false)
                    continue;

                string value = $"{item.Key.Trim()}:{item.Value.Trim()}";
                result.Add(value);
            }

            return result;
        }

        private IEnumerable<string> Get(IEnumerable<CapturedException> values)
        {
            List<string> result = new List<string>();

            result.AddRange(values.Select(p => p.Type));
            result.AddRange(values.Select(p => p.Message));

            return result;
        }

        private IEnumerable<string> GetFromInputStream(string inputStream)
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

            if (KissLogConfiguration.JsonSerializer.IsJson(inputStream))
            {
                var properties = KissLogConfiguration.JsonSerializer.DeserializeAndFlatten(inputStream);
                values = properties.Where(p => p.Value != null && IsSimpleType(p.Value.GetType())).Select(p => new KeyValuePair<string, string>(p.Key, p.Value.ToString())).ToList();
            }

            return Get(values);
        }

        private bool IsSimpleType(Type type)
        {
            return type.IsPrimitive
                || type.IsEnum
                || type == typeof(string)
                || type == typeof(Guid);
        }

        public class Options
        {
            public Criteria KeyCriteria { get; }
            public Criteria ValueCriteria { get; }

            public Options() : this(new Criteria(1, 100, KeyRegex), new Criteria(1, 100, ValueRegex))
            {

            }

            public Options(Criteria keyCriteria, Criteria valueCriteria)
            {
                KeyCriteria = keyCriteria ?? throw new ArgumentNullException(nameof(keyCriteria));
                ValueCriteria = valueCriteria ?? throw new ArgumentNullException(nameof(valueCriteria));
            }
        }

        public class Criteria
        {
            public int MinimumLength { get; }
            public int MaximumLength { get; }
            public Regex Pattern { get; }

            public Criteria(int minimumLength, int maximumLength) : this(minimumLength, maximumLength, null)
            {

            }

            public Criteria(int minimumLength, int maximumLength, Regex pattern)
            {
                if (minimumLength < 1)
                    throw new ArgumentException($"{nameof(minimumLength)} must be greater or equal to 1", nameof(minimumLength));

                if (maximumLength < minimumLength)
                    throw new ArgumentException($"{nameof(minimumLength)} must be greater or equal to {nameof(minimumLength)}", nameof(maximumLength));

                MinimumLength = minimumLength;
                MaximumLength = maximumLength;
                Pattern = pattern;
            }
        }
    }
}
