using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace KissLog.Tests
{
    [TestClass]
    public class InternalHelpersTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WrapInTryCatchThrowsExceptionForNullArgument()
        {
            InternalHelpers.WrapInTryCatch(null);
        }

        [TestMethod]
        public void WrapInTryCatchLogsException()
        {
            string messageArg = null;
            KissLogConfiguration.InternalLog = (message) => messageArg = message;

            string exceptionMessage = $"Exception {Guid.NewGuid()}";

            InternalHelpers.WrapInTryCatch(() => throw new Exception(exceptionMessage));

            Assert.IsNotNull(messageArg);
            Assert.IsTrue(messageArg.Contains(exceptionMessage));
        }

        [TestMethod]
        public void CanReadRequestInputStreamDoesNotThrowExceptionForNullHeaders()
        {
            var result = InternalHelpers.CanReadRequestInputStream(null);
        }

        [TestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow(" ", false)]
        [DataRow("application/javascript; charset=utf-8", true)]
        [DataRow("application/json; charset=utf-8", true)]
        [DataRow("application/xml; charset=utf-8", true)]
        [DataRow("text/html; charset=utf-8", true)]
        [DataRow("text/plain; charset=utf-8", true)]
        [DataRow("text/xml; charset=utf-8", true)]
        [DataRow("application/x-www-form-urlencoded", false)]
        [DataRow("multipart/form-data", false)]
        public void CanReadRequestInputStream(string contentType, bool canRead)
        {
            List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Content-Type", contentType)
            };

            var result = InternalHelpers.CanReadRequestInputStream(headers);

            Assert.AreEqual(canRead, result);
        }

        [TestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow(" ", false)]
        [DataRow("application/json; charset=utf-8", true)]
        [DataRow("application/xml; charset=utf-8", true)]
        [DataRow("text/plain; charset=utf-8", true)]
        [DataRow("text/xml; charset=utf-8", true)]
        [DataRow("text/html; charset=utf-8", true)]
        [DataRow("application/javascript; charset=utf-8", false)]
        [DataRow("application/x-www-form-urlencoded", false)]
        [DataRow("multipart/form-data", false)]
        [DataRow("application/pdf", false)]
        [DataRow("image/png", false)]
        public void CanReadResponseBody(string contentType, bool canRead)
        {
            List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Content-Type", contentType)
            };

            var result = InternalHelpers.CanReadResponseBody(headers);

            Assert.AreEqual(canRead, result);
        }

        [TestMethod]
        [DataRow(null, "Response.txt")]
        [DataRow("", "Response.txt")]
        [DataRow(" ", "Response.txt")]
        [DataRow("application/json; charset=utf-8", "Response.json")]
        [DataRow("application/xml; charset=utf-8", "Response.xml")]
        [DataRow("text/plain; charset=utf-8", "Response.txt")]
        [DataRow("text/xml; charset=utf-8", "Response.xml")]
        [DataRow("text/html; charset=utf-8", "Response.html")]
        [DataRow("application/javascript; charset=utf-8", "Response.txt")]
        [DataRow("application/x-www-form-urlencoded", "Response.txt")]
        [DataRow("multipart/form-data", "Response.txt")]
        [DataRow("application/pdf", "Response.txt")]
        [DataRow("image/png", "Response.txt")]
        public void GenerateResponseFileName(string contentType, string expectedFileName)
        {
            List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Content-Type", contentType)
            };

            var result = InternalHelpers.GenerateResponseFileName(headers);

            Assert.AreEqual(expectedFileName, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExplicitLogResponseBodyThrowsExceptionForNullList()
        {
            bool? value = InternalHelpers.GetExplicitLogResponseBody(null);
        }

        [TestMethod]
        public void GetExplicitLogResponseBodyReturnsNullForEmptyList()
        {
            bool? value = InternalHelpers.GetExplicitLogResponseBody(new List<Logger>());

            Assert.IsNull(value);
        }

        [TestMethod]
        public void GetExplicitLogResponseBodyReturnsNullIfNoValueIsFound()
        {
            Logger logger1 = new Logger();
            Logger logger2 = new Logger();

            bool? value = InternalHelpers.GetExplicitLogResponseBody(new List<Logger> { logger1, logger2 });

            Assert.IsNull(value);
        }

        [TestMethod]
        public void GetExplicitLogResponseBodyUsesDefaultLoggerAsPrimarySource()
        {
            Logger logger1 = new Logger(categoryName: "Category 1");
            Logger logger2 = new Logger();

            logger1.LogResponseBody(true);
            logger2.LogResponseBody(false);

            bool? value = InternalHelpers.GetExplicitLogResponseBody(new List<Logger> { logger1, logger2 });

            Assert.AreEqual(false, value.Value);
        }

        [TestMethod]
        public void GetExplicitLogResponseBodyReturnsTheFirstValueFound()
        {
            Logger logger1 = new Logger();
            Logger logger2 = new Logger();
            Logger logger3 = new Logger();

            logger2.LogResponseBody(false);
            logger3.LogResponseBody(true);

            bool? value = InternalHelpers.GetExplicitLogResponseBody(new List<Logger> { logger1, logger2, logger3 });

            Assert.AreEqual(false, value.Value);
        }
    }
}
