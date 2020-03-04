using KissLog.FlushArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace KissLog.Apis.v1.Listeners
{
    public class GenerateKeywordsService
    {
        private static readonly Regex KeyRegex = new Regex(@"^[a-zA-Z0-9_-]*$", RegexOptions.Compiled);
        private static readonly Regex ValueRegex = new Regex(@"^[a-zA-Z0-9_\-\+\/.@: ]*$", RegexOptions.Compiled);

        public bool IsEnabled { get; set; } = true;
        public int KeyMinLength { get; set; } = 1;
        public int KeyMaxLength { get; set; } = 100;
        public int ValueMinLength { get; set; } = 1;
        public int ValueMaxLength { get; set; } = 100;

        public IList<string> CreateKeywords(FlushLogArgs args)
        {
            if (!IsEnabled || args == null)
                return new List<string>();

            List<string> result = new List<string>();

            string path = args.WebProperties.Request.Url.LocalPath;
            // string tokenizedPath = args.WebProperties.Request.Url.TokenizedPath;
            string username = args.WebProperties.Request.User?.Name;
            string emailAddress = args.WebProperties.Request.User?.EmailAddress;

            result.Add(path);
            // result.Add(tokenizedPath);

            if (!string.IsNullOrEmpty(username))
                result.Add(username);

            if (!string.IsNullOrEmpty(emailAddress))
                result.Add(emailAddress);

            IEnumerable<string> parameters = GetFromParameters(args.WebProperties.Request.Properties.QueryString);
            result.AddRange(parameters);

            parameters = GetFromParameters(args.WebProperties.Request.Properties.FormData);
            result.AddRange(parameters);

            parameters = GetFromInputStream(args.WebProperties.Request.Properties.InputStream);
            result.AddRange(parameters);

            IEnumerable<string> exceptions = GetExceptions(args);
            result.AddRange(exceptions);

            result = result.Where(p => !string.IsNullOrEmpty(p)).Distinct().ToList();

            return result;
        }

        private List<string> GetExceptions(FlushLogArgs args)
        {
            if (args.CapturedExceptions == null || args.CapturedExceptions.Any() == false)
                return new List<string>();

            List<string> exceptionType = args.CapturedExceptions.Select(p => p.ExceptionType).ToList();
            List<string> exceptionMessage = args.CapturedExceptions.Select(p => p.ExceptionMessage).ToList();

            List<string> result = new List<string>();
            result.AddRange(exceptionType);
            result.AddRange(exceptionMessage);

            return result;
        }

        private List<string> GetFromParameters(List<KeyValuePair<string, string>> items)
        {
            if (items == null || !items.Any())
                return new List<string>();

            List<string> result =
                items
                    .Where(p => !string.IsNullOrEmpty(p.Key))
                    .Where(p => p.Key.Length >= KeyMinLength && p.Key.Length <= KeyMaxLength)
                    .Where(p => KeyRegex.IsMatch(p.Key))

                    .Where(p => !string.IsNullOrEmpty(p.Value))
                    .Where(p => p.Value.Length >= ValueMinLength && p.Value.Length <= ValueMaxLength)
                    .Where(p => ValueRegex.IsMatch(p.Value))

                    .Select(p => $"{p.Key.Trim()}:{p.Value.Trim()}")
                    .ToList();

            return result;
        }

        private List<string> GetFromInputStream(string inputStream)
        {
            if (string.IsNullOrWhiteSpace(inputStream))
                return new List<string>();

            List<KeyValuePair<string, object>> flattenInputStream = JsonUtilities.DeserializeAndFlatten(inputStream).ToList();

            // we filter InputStream to match only root elements of which value is a simple type (string, number) and not an object
            List<KeyValuePair<string, string>> inputStreamValues =
                flattenInputStream
                    .Where(p => !string.IsNullOrEmpty(p.Key) && !p.Key.Contains(".") && p.Value != null && IsSimple(p.Value.GetType()))
                    .Select(p => new KeyValuePair<string, string>(p.Key, p.Value.ToString()))
                    .ToList();

            return GetFromParameters(inputStreamValues);
        }

        private bool IsSimple(Type type)
        {
            #if NETSTANDARD1_3 || NETSTANDARD1_4

            return IsSiple_NetStandard(type);

            #else

            return type.IsPrimitive
                   || type.IsEnum
                   || type == typeof(string)
                   || type == typeof(Guid);

            #endif
        }

        private bool IsSiple_NetStandard(Type type)
        {
            if (type == null)
                return false;

            if(type == typeof(string) ||
               type == typeof(Guid))
            {
                return true;
            }

            TypeInfo typeInfo = type.GetTypeInfo();
            if (typeInfo == null)
                return false;

            return typeInfo.IsPrimitive || typeInfo.IsEnum;
        }
    }
}
