using KissLog.CloudListeners.Auth;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.CloudListeners.Tests.Auth
{
    [TestClass]
    public class ApplicationTests
    {
        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public void NullOrganizationIdDoesNotThrowException(string organizationId)
        {
            var application = new Application(organizationId, "applicationId");
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow(" ")]
        [DataRow("  ")]
        [DataRow(" organization-id ")]
        public void OrganizationIdIsTrimmed(string organizationId)
        {
            var application = new Application(organizationId, "applicationId");

            Assert.AreEqual(organizationId?.Trim(), application.OrganizationId);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public void NullApplicationIdDoesNotThrowException(string applicationId)
        {
            var application = new Application("organizationId", applicationId);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow(" ")]
        [DataRow("  ")]
        [DataRow(" application-id ")]
        public void ApplicationIdIsTrimmed(string applicationId)
        {
            var application = new Application("organizationId", applicationId);

            Assert.AreEqual(applicationId?.Trim(), application.ApplicationId);
        }

        [TestMethod]
        public void ConstructorUpdatesTheProperties()
        {
            var organizationId = Guid.NewGuid().ToString();
            var applicationId = Guid.NewGuid().ToString();

            var application = new Application(organizationId, applicationId);

            Assert.AreEqual(organizationId, application.OrganizationId);
            Assert.AreEqual(applicationId, application.ApplicationId);
        }
    }
}
