using KissLog.Http;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.Tests.OptionsArgsTests
{
    [TestClass]
    public class LogListenerClaimArgsTests
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
            var args = new KissLog.OptionsArgs.LogListenerClaimArgs(null, httpProperties, "claimType", "claimValue");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenHttpPropertiesIsNull()
        {
            var args = new KissLog.OptionsArgs.LogListenerClaimArgs(new CustomLogListener(), null, "claimType", "claimValue");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenHttpPropertiesResponseIsNull()
        {
            HttpProperties httpProperties = GetHttpProperties(false);
            var args = new KissLog.OptionsArgs.LogListenerClaimArgs(new CustomLogListener(), httpProperties, "claimType", "claimValue");
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenClaimTypeIsNull(string claimType)
        {
            HttpProperties httpProperties = GetHttpProperties(true);
            var args = new KissLog.OptionsArgs.LogListenerClaimArgs(new CustomLogListener(), httpProperties, claimType, "claimValue");
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void DoesNotThrowExceptionWhenClaimValueIsNull(string claimValue)
        {
            HttpProperties httpProperties = GetHttpProperties(true);
            var args = new KissLog.OptionsArgs.LogListenerClaimArgs(new CustomLogListener(), httpProperties, "claimType", claimValue);
        }
    }
}
