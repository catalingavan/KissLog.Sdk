using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.Tests
{
    [TestClass]
    public class KissLogPackageTests
    {
        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionForNullName(string name)
        {
            var package = new KissLogPackage(name, new Version());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionForNullVersion()
        {
            var package = new KissLogPackage("KissLog", null);
        }

        [TestMethod]
        public void ConstructorUpdatesProperties()
        {
            string packageName = $"KissLog-{Guid.NewGuid()}";
            Version version = Version.Parse("1.2.0");

            var package = new KissLogPackage(packageName, version);

            Assert.AreEqual(packageName, package.Name);
            Assert.AreEqual(version.ToString(), package.Version.ToString());
        }
    }
}
