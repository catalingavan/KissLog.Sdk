using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;

namespace KissLog.AspNet.Web.Tests.KissLogHttpModuleTests
{
    [TestClass]
    public class OnPostAuthenticateRquestTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionForNullHttpContext()
        {
            KissLogHttpModule module = new KissLogHttpModule();

            module.OnPostAuthenticateRquest(null);
        }

        [TestMethod]
        public void DoesNotThrowExceptionWhenUserIsNull()
        {
            var httpContext = Helpers.MockHttpContext();
            httpContext.SetupProperty(p => p.User, null);

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnPostAuthenticateRquest(httpContext.Object);
        }

        [TestMethod]
        public void DoesNotThrowExceptionWhenUserIsNotClaimsPrincipal()
        {
            var user = new Mock<IPrincipal>();

            var httpContext = Helpers.MockHttpContext();
            httpContext.Setup(p => p.User).Returns(user.Object);

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnPostAuthenticateRquest(httpContext.Object);
        }

        [TestMethod]
        public void DoesNotThrowExceptionWhenUserIdentityIsNull()
        {
            var user = new Mock<IPrincipal>();
            user.Setup(p => p.Identity).Returns((IIdentity)null);

            var httpContext = Helpers.MockHttpContext();
            httpContext.Setup(p => p.User).Returns(user.Object);

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnPostAuthenticateRquest(httpContext.Object);
        }

        [TestMethod]
        public void DoesNotThrowExceptionWhenUserIdentityIsNotClaimsIdentity()
        {
            var user = new Mock<IPrincipal>();
            user.Setup(p => p.Identity).Returns(new Mock<IIdentity>().Object);

            var httpContext = Helpers.MockHttpContext();
            httpContext.Setup(p => p.User).Returns(user.Object);

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnPostAuthenticateRquest(httpContext.Object);
        }

        [TestMethod]
        public void GetsOrAddsTheLoggerToHttpContext()
        {
            var user = new Mock<ClaimsPrincipal>();
            user.Setup(p => p.Identity).Returns(new Mock<ClaimsIdentity>().Object);

            var httpContext = Helpers.MockHttpContext();
            httpContext.Setup(p => p.User).Returns(user.Object);

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnPostAuthenticateRquest(httpContext.Object);

            var dictionary = httpContext.Object.Items[LoggerFactory.DictionaryKey] as IDictionary<string, Logger>;

            Assert.IsNotNull(dictionary);
            Assert.AreEqual(1, dictionary.Count);
        }

        [TestMethod]
        public void ServerVariablesAreCopied()
        {
            var value = KissLog.Tests.Common.CommonTestHelpers.GenerateList(5);

            var identity = new Mock<ClaimsIdentity>();
            identity.Setup(p => p.Claims).Returns(Helpers.GenerateClaims(value));

            var user = new Mock<ClaimsPrincipal>();
            user.Setup(p => p.Identity).Returns(identity.Object);

            var httpContext = Helpers.MockHttpContext();
            httpContext.Setup(p => p.User).Returns(user.Object);

            KissLogHttpModule module = new KissLogHttpModule();
            module.OnPostAuthenticateRquest(httpContext.Object);

            var dictionary = httpContext.Object.Items[LoggerFactory.DictionaryKey] as IDictionary<string, Logger>;
            Logger logger = dictionary[Constants.DefaultLoggerCategoryName];

            Assert.AreEqual(JsonSerializer.Serialize(value), JsonSerializer.Serialize(logger.DataContainer.HttpProperties.Request.Properties.Claims));
        }
    }
}
