using KissLog.Http;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace KissLog.Tests
{
    [TestClass]
    public class FlushLogArgsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullCreateOptionsThrowsException()
        {
            FlushLogArgs item = new FlushLogArgs(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenHttpPropertiesIsNull()
        {
            FlushLogArgs item = new FlushLogArgs(new FlushLogArgs.CreateOptions
            {
                HttpProperties = null
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenHttpPropertiesResponseIsNull()
        {
            HttpProperties httpProperties = new HttpProperties(new HttpRequest(new HttpRequest.CreateOptions
            {
                HttpMethod = "GET",
                Url = UrlParser.GenerateUri(null)
            }));

            FlushLogArgs item = new FlushLogArgs(new FlushLogArgs.CreateOptions
            {
                HttpProperties = httpProperties
            });
        }

        [TestMethod]
        public void MessagesGroupsIsNotNull()
        {
            FlushLogArgs item = new FlushLogArgs(new FlushLogArgs.CreateOptions
            {
                HttpProperties = CommonTestHelpers.Factory.CreateHttpProperties(),
                MessagesGroups = null
            });

            Assert.IsNotNull(item.MessagesGroups);
        }

        [TestMethod]
        public void ExceptionsIsNotNull()
        {
            FlushLogArgs item = new FlushLogArgs(new FlushLogArgs.CreateOptions
            {
                HttpProperties = CommonTestHelpers.Factory.CreateHttpProperties(),
                Exceptions = null
            });

            Assert.IsNotNull(item.Exceptions);
        }

        [TestMethod]
        public void FilesIsNotNull()
        {
            FlushLogArgs item = new FlushLogArgs(new FlushLogArgs.CreateOptions
            {
                HttpProperties = CommonTestHelpers.Factory.CreateHttpProperties(),
                Files = null
            });

            Assert.IsNotNull(item.Files);
        }

        [TestMethod]
        public void CloneCopiesAllTheProperties()
        {
            FlushLogArgs item = CommonTestHelpers.Factory.CreateFlushLogArgs();

            FlushLogArgs clone = item.Clone();

            Assert.AreEqual(System.Text.Json.JsonSerializer.Serialize(item), System.Text.Json.JsonSerializer.Serialize(clone));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetFilesThrowsExceptionForNull()
        {
            FlushLogArgs item = new FlushLogArgs(new FlushLogArgs.CreateOptions
            {
                HttpProperties = CommonTestHelpers.Factory.CreateHttpProperties()
            });

            item.SetFiles(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetMessagesGroupsThrowsExceptionForNull()
        {
            FlushLogArgs item = new FlushLogArgs(new FlushLogArgs.CreateOptions
            {
                HttpProperties = CommonTestHelpers.Factory.CreateHttpProperties()
            });

            item.SetMessagesGroups(null);
        }

        [TestMethod]
        public void SetFilesUpdatesProperty()
        {
            var files = Enumerable.Range(0, 2).Select(p => CommonTestHelpers.Factory.CreateLoggedFile()).ToList();

            FlushLogArgs item = new FlushLogArgs(new FlushLogArgs.CreateOptions
            {
                HttpProperties = CommonTestHelpers.Factory.CreateHttpProperties()
            });
            item.SetFiles(files);

            Assert.AreEqual(JsonSerializer.Serialize(files), JsonSerializer.Serialize(item.Files));
        }
    }
}
