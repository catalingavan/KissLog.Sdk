using KissLog.CloudListeners.RequestLogsListener;
using KissLog.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace KissLog.CloudListeners.Tests.RequestLogsListener
{
    [TestClass]
    public class OptionsExtensionMethodsTests
    {
        [TestMethod]
        public void CreateUserPayloadUpdatesHandlers()
        {
            Options options = new Options();

            Func<HttpRequest, KissLog.RestClient.Requests.CreateRequestLog.User> handler = (HttpRequest httpRequest) => new RestClient.Requests.CreateRequestLog.User();

            options.CreateUserPayload(handler);

            Assert.AreSame(handler, KissLog.CloudListeners.RequestLogsListener.RequestLogsApiListener.Options.Handlers.CreateUserPayload);
        }

        [TestMethod]
        public void GenerateSearchKeywordsUpdatesHandlers()
        {
            Options options = new Options();

            Func<FlushLogArgs, IEnumerable<string>> handler = (FlushLogArgs args) => new List<string>();

            options.GenerateSearchKeywords(handler);

            Assert.AreSame(handler, KissLog.CloudListeners.RequestLogsListener.RequestLogsApiListener.Options.Handlers.GenerateSearchKeywords);
        }
    }
}
