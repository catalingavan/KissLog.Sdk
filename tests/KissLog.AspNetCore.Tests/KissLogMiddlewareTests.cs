using KissLog.AspNetCore.ReadInputStream;
using KissLog.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KissLog.AspNetCore.Tests
{
    [TestClass]
    public class KissLogMiddlewareTests
    {
        [TestMethod]
        public async Task NotifiesOnBeginRequest()
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();

            List<KissLog.Http.HttpRequest> onBeginRequestArgs = new List<KissLog.Http.HttpRequest>();
            KissLogConfiguration.Listeners.Add(new KissLog.Tests.Common.CustomLogListener(onBeginRequest: (KissLog.Http.HttpRequest arg) => { onBeginRequestArgs.Add(arg); }));

            var context = Helpers.MockHttpContext();
            var middleware = Helpers.MockMiddleware();

            await middleware.Invoke(context.Object);

            Assert.AreEqual(1, onBeginRequestArgs.Count);
        }

        [TestMethod]
        public async Task NotifiesOnFlush()
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();

            List<FlushLogArgs> flushArgs = new List<FlushLogArgs>();
            KissLogConfiguration.Listeners.Add(new KissLog.Tests.Common.CustomLogListener(onFlush: (FlushLogArgs arg) => { flushArgs.Add(arg); }));

            var context = Helpers.MockHttpContext();
            var middleware = Helpers.MockMiddleware();

            await middleware.Invoke(context.Object);

            Assert.AreEqual(1, flushArgs.Count);
        }

        [TestMethod]
        public async Task EvaluatesOptionsShouldLogResponseBody()
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();

            KissLogConfiguration.Options.ShouldLogResponseBody((HttpProperties args) => false);

            LoggedFile file = null;
            KissLogConfiguration.Listeners.Add(new KissLog.Tests.Common.CustomLogListener(onFlush: (FlushLogArgs arg) =>
            {
                file = arg.Files.FirstOrDefault();
            }));

            var context = Helpers.MockHttpContext();
            var middleware = Helpers.MockMiddleware();

            await middleware.Invoke(context.Object);

            Assert.IsNull(file);
        }

        [TestMethod]
        public async Task DisposesTheResponseMirrorStreamDecorator()
        {
            var context = Helpers.MockHttpContext();
            var middleware = Helpers.MockMiddleware();

            await middleware.Invoke(context.Object);

            Assert.IsInstanceOfType(context.Object.Response.Body, typeof(MirrorStreamDecorator));
            Assert.IsTrue(((MirrorStreamDecorator)context.Object.Response.Body).MirrorStream.CanRead == false);
        }

        [TestMethod]
        public async Task GetsOrAddsTheLoggerToHttpContext()
        {
            var context = Helpers.MockHttpContext();
            var middleware = Helpers.MockMiddleware();

            await middleware.Invoke(context.Object);

            var dictionary = context.Object.Items[LoggerFactory.DictionaryKey] as IDictionary<string, Logger>;

            Assert.IsNotNull(dictionary);
            Assert.AreEqual(1, dictionary.Count);
        }

        [TestMethod]
        [DataRow("application/json")]
        [DataRow("application/xml")]
        [DataRow("text/plain")]
        [DataRow("text/xml")]
        public async Task AllowedContentTypeLogsTheResponseBodyAsAFile(string contentType)
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();
            KissLogConfiguration.Options.ShouldLogResponseBody((HttpProperties httpProperties) => true);

            ModuleInitializer.ReadInputStreamProvider = new EnableBufferingReadInputStreamProvider();

            LoggedFile file = null;
            string fileContent = null;
            KissLogConfiguration.Listeners.Add(new KissLog.Tests.Common.CustomLogListener(onFlush: (FlushLogArgs arg) =>
            {
                file = arg.Files.FirstOrDefault();
                fileContent = file == null ? null : File.ReadAllText(file.FilePath);
            }));

            string responseBody = $"ResponseBody {Guid.NewGuid()}";

            var context = Helpers.MockHttpContext(responseContentType: contentType);
            var middleware = Helpers.MockMiddleware(responseBody: responseBody);

            await middleware.Invoke(context.Object);

            Assert.IsNotNull(file);
            Assert.AreEqual(responseBody, fileContent);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("application/pdf")]
        [DataRow("application/octet-stream")]
        [DataRow("image/png")]
        public async Task NotAllowedContentTypeDoesNotLogTheResponseBody(string contentType)
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();

            LoggedFile file = null;
            KissLogConfiguration.Listeners.Add(new KissLog.Tests.Common.CustomLogListener(onFlush: (FlushLogArgs arg) =>
            {
                file = arg.Files.FirstOrDefault();
            }));

            var context = Helpers.MockHttpContext(responseContentType: contentType);
            var middleware = Helpers.MockMiddleware();

            await middleware.Invoke(context.Object);

            Assert.IsNull(file);
        }

        [TestMethod]
        [DataRow(100)]
        [DataRow(200)]
        [DataRow(201)]
        [DataRow(404)]
        [DataRow(500)]
        public async Task UpdatesTheStatusCode(int statusCode)
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();

            FlushLogArgs flushLogArgs = null;
            KissLogConfiguration.Listeners.Add(new KissLog.Tests.Common.CustomLogListener(onFlush: (FlushLogArgs arg) =>
            {
                flushLogArgs = arg;
            }));

            var context = Helpers.MockHttpContext();
            context.Setup(p => p.Response.StatusCode).Returns(statusCode);

            var middleware = Helpers.MockMiddleware();

            await middleware.Invoke(context.Object);

            Assert.AreEqual(statusCode, flushLogArgs.HttpProperties.Response.StatusCode);
        }

        [TestMethod]
        public async Task LogsTheException()
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();

            var ex = new Exception($"Exception {Guid.NewGuid()}");

            FlushLogArgs flushLogArgs = null;
            KissLogConfiguration.Listeners.Add(new KissLog.Tests.Common.CustomLogListener(onFlush: (FlushLogArgs arg) =>
            {
                flushLogArgs = arg;
            }));

            var context = Helpers.MockHttpContext();

            var middleware = new KissLogMiddleware((innerHttpContext) =>
            {
                throw ex;
            });

            try
            {
                await middleware.Invoke(context.Object);
            }
            catch
            {
                // ignored
            }

            CapturedException capturedException = flushLogArgs.Exceptions.First();
            LogMessage message = flushLogArgs.MessagesGroups.First().Messages.First();

            Assert.AreEqual(ex.Message, capturedException.Message);
            Assert.IsTrue(message.Message.Contains(ex.Message));
        }

        [TestMethod]
        public async Task ExceptionSetsTheStatusCodeTo500()
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();

            FlushLogArgs flushLogArgs = null;
            KissLogConfiguration.Listeners.Add(new KissLog.Tests.Common.CustomLogListener(onFlush: (FlushLogArgs arg) =>
            {
                flushLogArgs = arg;
            }));

            var context = Helpers.MockHttpContext();
            context.Setup(p => p.Response.StatusCode).Returns(200);

            var middleware = new KissLogMiddleware((innerHttpContext) =>
            {
                throw new Exception();
            });

            try
            {
                await middleware.Invoke(context.Object);
            }
            catch
            {
                // ignored
            }

            Assert.AreEqual(500, flushLogArgs.HttpProperties.Response.StatusCode);
        }

        [TestMethod]
        public void UpdatesTheLoggerFactory()
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();

            var middleware = new KissLogMiddleware((innerHttpContext) =>
            {
                return null;
            });

            Assert.IsNotNull(Logger.Factory);
            Assert.IsInstanceOfType(Logger.Factory, typeof(KissLog.AspNetCore.LoggerFactory));
        }
    }
}
