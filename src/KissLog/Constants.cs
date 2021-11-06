using System;
using System.Text.RegularExpressions;

namespace KissLog
{
    public class Constants
    {
        public static readonly Regex FileNameRegex =                            new Regex(@"[^a-zA-Z0-9_\-\+\. ]+", RegexOptions.Compiled);
        public static readonly Regex UrlRegex =                                 new Regex(@"[^a-zA-Z0-9/:._\+\=\&\-\?]+", RegexOptions.Compiled);
        public static readonly Regex HttpMethodRegex =                          new Regex(@"[^a-zA-Z0-9]+", RegexOptions.Compiled);

        public static readonly string[] ReadInputStreamContentTypes =           new[] { "application/javascript", "application/json", "application/xml", "text/plain", "text/xml", "text/html" };
        public static readonly string[] ReadResponseBodyContentTypes =          new[] { "application/json", "application/xml", "text/html", "text/plain", "text/xml" };
        public static readonly string[] DefaultReadResponseBodyContentTypes =   new[] { "application/json" };

        public const string DefaultLoggerCategoryName =                         "Default";
        public const long   MaximumAllowedFileSizeInBytes =                     5 * 1024 * 1024;
        public const long   ReadStreamAsStringMaxContentLengthInBytes =         1 * 1024 * 1024;
        public const string DefaultBaseUrl =                                    "http://application";

        internal static KissLogPackage UnknownKissLogPackage => new KissLogPackage("KissLog.Unknown", new Version(1, 0, 0));
    }
}
