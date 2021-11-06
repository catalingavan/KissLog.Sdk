using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.Tests
{
    [TestClass]
    public class KissLogPackagesContainerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddThrowsExceptionForNullArgument()
        {
            var container = new KissLogPackagesContainer();
            container.Add(null);
        }

        [TestMethod]
        public void AddInsertsANewRecord()
        {
            string name = $"KissLog.{Guid.NewGuid()}";
            Version version = new Version(1, 9, 0);

            var container = new KissLogPackagesContainer();
            container.Add(new KissLogPackage(name, version));

            IEnumerable<KissLogPackage> packages = container.GetAll();

            Assert.AreEqual(1, packages.Count());
            Assert.AreEqual(name, packages.ElementAt(0).Name);
            Assert.AreEqual(version, packages.ElementAt(0).Version);
        }

        [TestMethod]
        public void AddDoesNotInsertANewRecordIfAPackageWithTheSameNameAndGteVersionAlreadyExists()
        {
            string name = $"KissLog.{Guid.NewGuid()}";

            var container = new KissLogPackagesContainer();
            container.Add(new KissLogPackage(name, new Version(1, 1, 0)));
            container.Add(new KissLogPackage(name, new Version(1, 0, 0)));

            IEnumerable<KissLogPackage> packages = container.GetAll();

            Assert.AreEqual(1, packages.Count());
            Assert.AreEqual(name, packages.ElementAt(0).Name);
            Assert.AreEqual(new Version(1, 1, 0), packages.ElementAt(0).Version);
        }

        [TestMethod]
        public void AddReplacesTheExistingRecordIfAPackageWithTheSameNameAndLtVersionAlreadyExists()
        {
            string name = $"KissLog.{Guid.NewGuid()}";

            var container = new KissLogPackagesContainer();
            container.Add(new KissLogPackage(name, new Version(1, 0, 0)));
            container.Add(new KissLogPackage(name, new Version(1, 1, 0)));

            IEnumerable<KissLogPackage> packages = container.GetAll();

            Assert.AreEqual(1, packages.Count());
            Assert.AreEqual(name, packages.ElementAt(0).Name);
            Assert.AreEqual(new Version(1, 1, 0), packages.ElementAt(0).Version);
        }

        [TestMethod]
        public void AddDoesNothingIfAPackageWithTheSameNameAndVersionAlreadyExists()
        {
            string name = $"KissLog.{Guid.NewGuid()}";

            var container = new KissLogPackagesContainer();
            container.Add(new KissLogPackage(name, new Version(1, 1, 0)));
            container.Add(new KissLogPackage(name, new Version(1, 1, 0)));

            IEnumerable<KissLogPackage> packages = container.GetAll();

            Assert.AreEqual(1, packages.Count());
            Assert.AreEqual(name, packages.ElementAt(0).Name);
            Assert.AreEqual(new Version(1, 1, 0), packages.ElementAt(0).Version);
        }

        [TestMethod]
        public void GetPrimaryPackageReturnsUnknownKissLogPackageIfNoPackageWasFound()
        {
            var container = new KissLogPackagesContainer();
            KissLogPackage package = container.GetPrimaryPackage();

            Assert.AreEqual(Constants.UnknownKissLogPackage.Name, package.Name);
            Assert.AreEqual(Constants.UnknownKissLogPackage.Version, package.Version);
        }

        [TestMethod]
        public void GetPrimaryPackageReturnsTheFirstMatchingNonKissLogPackage()
        {
            var container = new KissLogPackagesContainer();

            container.Add(new KissLogPackage("KissLog.WebApi", new Version(1, 5, 0)));
            container.Add(new KissLogPackage("KissLog.AspNetCore", new Version(2, 0, 0)));
            container.Add(new KissLogPackage("KissLog", new Version(1, 0, 0)));

            KissLogPackage package = container.GetPrimaryPackage();

            Assert.AreEqual("KissLog.AspNetCore", package.Name);
            Assert.AreEqual(new Version(2, 0, 0), package.Version);
        }
    }
}
