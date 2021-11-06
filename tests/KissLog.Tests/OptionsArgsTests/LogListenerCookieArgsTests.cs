using KissLog.Http;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.Tests.OptionsArgsTests
{
    [TestClass]
    public class LogListenerCookieArgsTests
    {
        private HttpProperties GetHttpProperties(bool includeResponse)
        {
            HttpProperties httpProperties = new HttpProperties(new HttpRequest(new HttpRequest.CreateOptions
            {
                HttpMethod = "GET",
                Url = UrlParser.GenerateUri(null)
            }));

            if (includeResponse)
            {
                httpProperties.SetResponse(new HttpResponse(new HttpResponse.CreateOptions() { }));
            }

            return httpProperties;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenListenerIsNull()
        {
            HttpProperties httpProperties = GetHttpProperties(true);
            var args = new KissLog.OptionsArgs.LogListenerCookieArgs(null, httpProperties, "cookieName", "cookieValue");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenHttpPropertiesIsNull()
        {
            var args = new KissLog.OptionsArgs.LogListenerCookieArgs(new CustomLogListener(), null, "cookieName", "cookieValue");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenHttpPropertiesResponseIsNull()
        {
            HttpProperties httpProperties = GetHttpProperties(false);
            var args = new KissLog.OptionsArgs.LogListenerCookieArgs(new CustomLogListener(), httpProperties, "cookieName", "cookieValue");
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenCookieNameIsNull(string cookieName)
        {
            HttpProperties httpProperties = GetHttpProperties(true);
            var args = new KissLog.OptionsArgs.LogListenerCookieArgs(new CustomLogListener(), httpProperties, cookieName, "cookieValue");
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void DoesNotThrowExceptionWhenCookieValueIsNull(string cookieValue)
        {
            HttpProperties httpProperties = GetHttpProperties(true);
            var args = new KissLog.OptionsArgs.LogListenerCookieArgs(new CustomLogListener(), httpProperties, "cookieName", cookieValue);
        }
    }
}
