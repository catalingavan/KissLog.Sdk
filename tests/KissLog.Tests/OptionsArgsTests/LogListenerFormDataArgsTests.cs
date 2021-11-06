using KissLog.Http;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.Tests.OptionsArgsTests
{
    [TestClass]
    public class LogListenerFormDataArgsTests
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
            var args = new KissLog.OptionsArgs.LogListenerFormDataArgs(null, httpProperties, "formDataName", "formDataValue");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenHttpPropertiesIsNull()
        {
            var args = new KissLog.OptionsArgs.LogListenerFormDataArgs(new CustomLogListener(), null, "formDataName", "formDataValue");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenHttpPropertiesResponseIsNull()
        {
            HttpProperties httpProperties = GetHttpProperties(false);
            var args = new KissLog.OptionsArgs.LogListenerFormDataArgs(new CustomLogListener(), httpProperties, "formDataName", "formDataValue");
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenFormDataNameIsNull(string formDataName)
        {
            HttpProperties httpProperties = GetHttpProperties(true);
            var args = new KissLog.OptionsArgs.LogListenerFormDataArgs(new CustomLogListener(), httpProperties, formDataName, "formDataValue");
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void DoesNotThrowExceptionWhenFormDataValueIsNull(string formDataValue)
        {
            HttpProperties httpProperties = GetHttpProperties(true);
            var args = new KissLog.OptionsArgs.LogListenerFormDataArgs(new CustomLogListener(), httpProperties, "formDataName", formDataValue);
        }
    }
}
