using KissLog.Http;
using KissLog.NotifyListeners;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace KissLog.Tests.NotifyListeners
{
    [TestClass]
    public class NotifyBeginRequestTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenHttpRequestIsNull()
        {
            NotifyBeginRequest.Notify(null);
        }

        [TestMethod]
        public void NotifyIsInvokedForEachLogListener()
        {
            CommonTestHelpers.ResetContext();

            List<HttpRequest> httpRequestArgs = new List<HttpRequest>();

            KissLogConfiguration.Listeners.Add(new CustomLogListener(onBeginRequest: (HttpRequest arg) => { httpRequestArgs.Add(arg); }));
            KissLogConfiguration.Listeners.Add(new CustomLogListener(onBeginRequest: (HttpRequest arg) => { httpRequestArgs.Add(arg); }));
            KissLogConfiguration.Listeners.Add(new CustomLogListener(onBeginRequest: (HttpRequest arg) => { httpRequestArgs.Add(arg); }));

            HttpRequest httpRequest = new HttpRequest(new HttpRequest.CreateOptions
            {
                Url = UrlParser.GenerateUri(null),
                HttpMethod = "GET"
            });

            NotifyBeginRequest.Notify(httpRequest);

            Assert.AreEqual(3, httpRequestArgs.Count);
        }

        [TestMethod]
        public void NotifyContinuesForOtherListenersWhenOneThrowsAnException()
        {
            CommonTestHelpers.ResetContext();

            List<HttpRequest> httpRequestArgs = new List<HttpRequest>();

            KissLogConfiguration.Listeners.Add(new CustomLogListener(onBeginRequest: (HttpRequest arg) => { throw new Exception(); }));
            KissLogConfiguration.Listeners.Add(new CustomLogListener(onBeginRequest: (HttpRequest arg) => { httpRequestArgs.Add(arg); }));
            KissLogConfiguration.Listeners.Add(new CustomLogListener(onBeginRequest: (HttpRequest arg) => { httpRequestArgs.Add(arg); }));

            HttpRequest httpRequest = new HttpRequest(new HttpRequest.CreateOptions
            {
                Url = UrlParser.GenerateUri(null),
                HttpMethod = "GET"
            });

            NotifyBeginRequest.Notify(httpRequest);

            Assert.AreEqual(2, httpRequestArgs.Count);
        }

        [TestMethod]
        public void LogListenerInterceptorShouldLogIsEvaluated()
        {
            CommonTestHelpers.ResetContext();

            List<HttpRequest> httpRequestArgs = new List<HttpRequest>();

            ILogListener listener = new CustomLogListener(onBeginRequest: (arg) => { httpRequestArgs.Add(arg); })
            {
                Interceptor = new CustomLogListenerInterceptor
                {
                    ShouldLogBeginRequest = (HttpRequest httpRequest) =>
                    {
                        return httpRequest.Url.LocalPath == "/App/Method1" || httpRequest.Url.LocalPath == "/App/Method3";
                    }
                }
            };

            KissLogConfiguration.Listeners.Add(listener);

            string[] urls = new[] { "/App/Method1", "/App/Method2", "/App/Method3", "/App/Method4" };

            foreach (string url in urls)
            {
                HttpRequest httpRequest = new HttpRequest(new HttpRequest.CreateOptions
                {
                    Url = UrlParser.GenerateUri(url),
                    HttpMethod = "GET"
                });

                NotifyBeginRequest.Notify(httpRequest);
            }

            Assert.AreEqual(2, httpRequestArgs.Count);
        }
    }
}
