using KissLog.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace KissLog.AspNet.Web.Tests.KissLogHttpModuleTests
{
    [TestClass]
    public class OnEndRequestTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionForNullHttpContext()
        {
            KissLogHttpModule module = new KissLogHttpModule();

            module.OnEndRequest(null);
        }

        [TestMethod]
        public void GetsOrAddsTheLoggerToHttpContext()
        {
            var httpContext = Helpers.MockHttpContext();

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnEndRequest(httpContext.Object);

            var dictionary = httpContext.Object.Items[LoggerFactory.DictionaryKey] as IDictionary<string, Logger>;

            Assert.IsNotNull(dictionary);
            Assert.AreEqual(1, dictionary.Count);
        }

        [TestMethod]
        public void GetsUpdatesTheLoggerHttpResponse()
        {
            var httpContext = Helpers.MockHttpContext();

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnEndRequest(httpContext.Object);

            var dictionary = httpContext.Object.Items[LoggerFactory.DictionaryKey] as IDictionary<string, Logger>;

            Logger logger = dictionary[Constants.DefaultLoggerCategoryName];

            Assert.AreEqual(logger.DataContainer.HttpProperties.Response.StatusCode, 204);
        }

        [TestMethod]
        [DataRow("application/json")]
        [DataRow("application/xml")]
        [DataRow("text/plain")]
        [DataRow("text/xml")]
        public void AllowedContentTypeLogsTheResponseBodyAsAFile(string contentType)
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();
            KissLogConfiguration.Options.ShouldLogResponseBody((HttpProperties httpProperties) => true);

            LoggedFile file = null;
            string fileContent = null;
            KissLogConfiguration.Listeners.Add(new KissLog.Tests.Common.CustomLogListener(onFlush: (FlushLogArgs arg) =>
            {
                file = arg.Files.FirstOrDefault();
                fileContent = file == null ? null : File.ReadAllText(file.FilePath);
            }));

            string responseBody = $"ResponseBody {Guid.NewGuid()}";

            var httpContext = Helpers.MockHttpContext(responseBody: responseBody);
            httpContext.Setup(p => p.Response.Headers).Returns(Helpers.GenerateNameValueCollection(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Content-Type", contentType)
            }));

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnEndRequest(httpContext.Object);

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
        public void NotAllowedContentTypeDoesNotLogTheResponseBody(string contentType)
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();

            LoggedFile file = null;
            KissLogConfiguration.Listeners.Add(new KissLog.Tests.Common.CustomLogListener(onFlush: (FlushLogArgs arg) =>
            {
                file = arg.Files.FirstOrDefault();
            }));

            var httpContext = Helpers.MockHttpContext();
            httpContext.Setup(p => p.Response.Headers).Returns(Helpers.GenerateNameValueCollection(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Content-Type", contentType)
            }));

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnEndRequest(httpContext.Object);

            Assert.IsNull(file);
        }

        [TestMethod]
        public void DisposesTheResponseMirrorStreamDecorator()
        {
            var ms = new MirrorStreamDecorator(new MemoryStream());

            var httpResponse = new Mock<HttpResponseBase>();
            httpResponse.SetupProperty(p => p.Filter, ms);

            var httpContext = Helpers.MockHttpContext();
            httpContext.Setup(p => p.Response).Returns(httpResponse.Object);

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnEndRequest(httpContext.Object);

            Assert.IsTrue(ms.MirrorStream.CanRead == false);
        }

        [TestMethod]
        public void NotifiesListeners()
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();

            List<FlushLogArgs> flushArgs = new List<FlushLogArgs>();
            KissLogConfiguration.Listeners.Add(new KissLog.Tests.Common.CustomLogListener(onFlush: (FlushLogArgs arg) => { flushArgs.Add(arg); }));

            var httpContext = Helpers.MockHttpContext();

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnEndRequest(httpContext.Object);

            Assert.AreEqual(1, flushArgs.Count);
        }

        [TestMethod]
        public void EvaluatesOptionsShouldLogResponseBody()
        {
            KissLog.Tests.Common.CommonTestHelpers.ResetContext();

            KissLogConfiguration.Options.ShouldLogResponseBody((HttpProperties args) => false);

            LoggedFile file = null;
            KissLogConfiguration.Listeners.Add(new KissLog.Tests.Common.CustomLogListener(onFlush: (FlushLogArgs arg) =>
            {
                file = arg.Files.FirstOrDefault();
            }));

            var httpContext = Helpers.MockHttpContext();

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnEndRequest(httpContext.Object);

            Assert.IsNull(file);
        }
    }
}
