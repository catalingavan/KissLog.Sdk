using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Web;

namespace KissLog.AspNet.Web.Tests.KissLogHttpModuleTests
{
    [TestClass]
    public class PostAcquireRequestStateTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionForNullHttpContext()
        {
            KissLogHttpModule module = new KissLogHttpModule();
            module.PostAcquireRequestState(null);
        }

        [TestMethod]
        public void DoesNotThrowExceptionWhenSessionIsNull()
        {
            var httpContext = Helpers.MockHttpContext();
            httpContext.Setup(p => p.Session).Returns((HttpSessionStateBase)null);

            KissLogHttpModule module = new KissLogHttpModule();
            module.PostAcquireRequestState(httpContext.Object);
        }

        [TestMethod]
        public void AddsSessionKey()
        {
            string sessionKey = null;

            var session = new Mock<HttpSessionStateBase>();
            session.Setup(p => p.Add(It.IsAny<string>(), It.IsAny<object>())).Callback((string name, object value) =>
            {
                sessionKey = name;
            });

            var httpContext = Helpers.MockHttpContext();
            httpContext.Setup(p => p.Session).Returns(session.Object);

            KissLogHttpModule module = new KissLogHttpModule();
            module.PostAcquireRequestState(httpContext.Object);

            Assert.AreEqual(KissLogHttpModule.SessionKey, sessionKey);
        }

        [TestMethod]
        public void GetsOrAddsTheLoggerToHttpContext()
        {
            var session = new Mock<HttpSessionStateBase>();

            var httpContext = Helpers.MockHttpContext();
            httpContext.Setup(p => p.Session).Returns(session.Object);

            KissLogHttpModule module = new KissLogHttpModule();
            module.PostAcquireRequestState(httpContext.Object);

            var dictionary = httpContext.Object.Items[LoggerFactory.DictionaryKey] as IDictionary<string, Logger>;

            Assert.IsNotNull(dictionary);
            Assert.AreEqual(1, dictionary.Count);
        }

        [TestMethod]
        public void SessionPropertiesAreCopied()
        {
            string sessionId = $"Session {Guid.NewGuid()}";
            bool isNewSession = true;

            var session = new Mock<HttpSessionStateBase>();
            session.Setup(p => p.SessionID).Returns(sessionId);
            session.Setup(p => p.IsNewSession).Returns(isNewSession);

            var httpContext = Helpers.MockHttpContext();
            httpContext.Setup(p => p.Session).Returns(session.Object);

            KissLogHttpModule module = new KissLogHttpModule();
            module.PostAcquireRequestState(httpContext.Object);

            var dictionary = httpContext.Object.Items[LoggerFactory.DictionaryKey] as IDictionary<string, Logger>;
            Logger logger = dictionary[Constants.DefaultLoggerCategoryName];

            Assert.AreEqual(sessionId, logger.DataContainer.HttpProperties.Request.SessionId);
            Assert.AreEqual(isNewSession, logger.DataContainer.HttpProperties.Request.IsNewSession);
        }
    }
}
