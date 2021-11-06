using KissLog.Http;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.Tests.OptionsArgsTests
{
    [TestClass]
    public class LogListenerServerVariableArgsTests
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
            var args = new KissLog.OptionsArgs.LogListenerServerVariableArgs(null, httpProperties, "serverVariableName", "serverVariableValue");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenHttpPropertiesIsNull()
        {
            var args = new KissLog.OptionsArgs.LogListenerServerVariableArgs(new CustomLogListener(), null, "serverVariableName", "serverVariableValue");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenHttpPropertiesResponseIsNull()
        {
            HttpProperties httpProperties = GetHttpProperties(false);
            var args = new KissLog.OptionsArgs.LogListenerServerVariableArgs(new CustomLogListener(), httpProperties, "serverVariableName", "serverVariableValue");
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenServerVariableNameIsNull(string serverVariableName)
        {
            HttpProperties httpProperties = GetHttpProperties(true);
            var args = new KissLog.OptionsArgs.LogListenerServerVariableArgs(new CustomLogListener(), httpProperties, serverVariableName, "serverVariableValue");
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void DoesNotThrowExceptionWhenServerVariableValueIsNull(string serverVariableValue)
        {
            HttpProperties httpProperties = GetHttpProperties(true);
            var args = new KissLog.OptionsArgs.LogListenerServerVariableArgs(new CustomLogListener(), httpProperties, "serverVariableName", serverVariableValue);
        }
    }
}
