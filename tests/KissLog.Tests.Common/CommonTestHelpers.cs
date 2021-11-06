using KissLog.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace KissLog.Tests.Common
{
    public class CommonTestHelpers
    {
        static ILoggerFactory _loggerFactory = null;
        static CommonTestHelpers()
        {
            _loggerFactory = Logger.Factory;
        }

        public static void ResetContext()
        {
            PropertyInfo prop = typeof(KissLogConfiguration).GetProperty("Listeners");
            prop.SetValue(KissLogConfiguration.Listeners, new LogListenersContainer());

            prop = typeof(KissLogConfiguration).GetProperty("Options");
            prop.SetValue(KissLogConfiguration.Options, new Options());

            prop = typeof(KissLogConfiguration).GetProperty("KissLogPackages", BindingFlags.Static | BindingFlags.NonPublic);
            prop.SetValue(KissLogConfiguration.KissLogPackages, new KissLogPackagesContainer());

            KissLogConfiguration.InternalLog = (message) => Debug.WriteLine(message);

            Logger.Factory = _loggerFactory;
        }

        public static List<KeyValuePair<string, string>> GenerateList(int count)
        {
            return Enumerable.Range(0, count).Select((p, i) => new KeyValuePair<string, string>($"Key {i}", $"Value-{Guid.NewGuid()}")).ToList();
        }

        public static List<KeyValuePair<string, string>> GenerateList(string keyName, int count)
        {
            return Enumerable.Range(0, count).Select((p, i) => new KeyValuePair<string, string>(keyName, $"Value-{Guid.NewGuid()}")).ToList();
        }

        public static Dictionary<string, string> GenerateDictionary(int count)
        {
            var items = GenerateList(count);
            return GenerateDictionary(items);
        }

        public static Dictionary<string, string> GenerateDictionary(List<KeyValuePair<string, string>> items)
        {
            return items.ToDictionary(p => p.Key, p => p.Value);
        }

        public static DirectoryInfo FindTestDataDirectory()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            DirectoryInfo di = new DirectoryInfo(path);

            while (string.Compare(di.Name, "tests", true) != 0)
            {
                di = di.Parent;
                if (di == null)
                    break;
            }

            di = di?.EnumerateDirectories("testData", SearchOption.TopDirectoryOnly)?.FirstOrDefault();
            if (di == null || !di.Exists)
                throw new DirectoryNotFoundException("testData directory was not found");

            return di;
        }

        public class Factory
        {
            public static CapturedException CreateCapturedException()
            {
                return new CapturedException(new FileNotFoundException($"Message-{Guid.NewGuid()}", $"FileName-{Guid.NewGuid()}"));
            }

            public static LoggedFile CreateLoggedFile()
            {
                return new LoggedFile($"FileName-{Guid.NewGuid()}", $"FilePath-{Guid.NewGuid()}", 150);
            }

            public static LogMessage CreateLogMessage()
            {
                return new LogMessage(new LogMessage.CreateOptions
                {
                    CategoryName = Guid.NewGuid().ToString(),
                    LogLevel = LogLevel.Information,
                    Message = $"Message {Guid.NewGuid()}",
                    MemberType = $"MemberType {Guid.NewGuid()}",
                    MemberName = $"MemberName {Guid.NewGuid()}",
                    LineNumber = 100,
                    DateTime = DateTime.UtcNow.AddSeconds(-90)
                });
            }

            public static LogMessagesGroup CreateLogMessagesGroup()
            {
                var messages = Enumerable.Range(0, 2).Select(p => CreateLogMessage()).ToList();
                return new LogMessagesGroup($"Category-{Guid.NewGuid()}", messages);
            }

            public static RequestProperties CreateRequestProperties()
            {
                return new RequestProperties(new RequestProperties.CreateOptions
                {
                    Headers = GenerateList(2),
                    Cookies = GenerateList(2),
                    QueryString = GenerateList(2),
                    FormData = GenerateList(2),
                    ServerVariables = GenerateList(2),
                    Claims = GenerateList(2),
                    InputStream = $"Input stream {Guid.NewGuid()}"
                });
            }

            public static ResponseProperties CreateResponseProperties()
            {
                return new ResponseProperties(new ResponseProperties.CreateOptions
                {
                    Headers = GenerateList(2),
                    ContentLength = 10000
                });
            }

            public static HttpRequest CreateHttpRequest()
            {
                return new HttpRequest(new HttpRequest.CreateOptions
                {
                    StartDateTime = DateTime.UtcNow.AddSeconds(-120),
                    Url = UrlParser.GenerateUri(Guid.NewGuid().ToString()),
                    HttpMethod = "GET",
                    UserAgent = $"UserAgent {Guid.NewGuid()}",
                    RemoteAddress = $"RemoteAddress {Guid.NewGuid()}",
                    HttpReferer = $"HttpReferer {Guid.NewGuid()}",
                    SessionId = $"SessionId {Guid.NewGuid()}",
                    IsNewSession = true,
                    IsAuthenticated = true,
                    MachineName = $"MachineName {Guid.NewGuid()}",
                    Properties = CreateRequestProperties()
                });
            }

            public static HttpResponse CreateHttpResponse()
            {
                return new HttpResponse(new HttpResponse.CreateOptions
                {
                    StatusCode = 100,
                    Properties = CreateResponseProperties()
                });
            }

            public static HttpProperties CreateHttpProperties()
            {
                var result = new HttpProperties(CreateHttpRequest());
                result.SetResponse(CreateHttpResponse());

                return result;
            }

            public static FlushLogArgs CreateFlushLogArgs()
            {
                return new FlushLogArgs(new FlushLogArgs.CreateOptions
                {
                    Exceptions = Enumerable.Range(0, 2).Select(p => CreateCapturedException()).ToList(),
                    Files = Enumerable.Range(0, 2).Select(p => CreateLoggedFile()).ToList(),
                    MessagesGroups = Enumerable.Range(0, 2).Select(p => CreateLogMessagesGroup()).ToList(),
                    HttpProperties = CreateHttpProperties(),
                    CustomProperties = GenerateList(2).Select(p => new KeyValuePair<string, object>(p.Key, (object)p.Value)).ToList(),
                    IsCreatedByHttpRequest = true
                });
            }
        }
    }
}
