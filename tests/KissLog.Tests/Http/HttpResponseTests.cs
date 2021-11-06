using KissLog.Http;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Text.Json;

namespace KissLog.Tests.Http
{
    [TestClass]
    public class HttpResponseTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullCreateOptionsThrowsException()
        {
            HttpResponse item = new HttpResponse(null);
        }

        [TestMethod]
        public void CreateOptionsConstructorUpdatesProperties()
        {
            var options = new HttpResponse.CreateOptions
            {
                StatusCode = (int)HttpStatusCode.NoContent,
                Properties = new ResponseProperties(new ResponseProperties.CreateOptions())
            };

            HttpResponse item = new HttpResponse(options);

            Assert.AreEqual(options.StatusCode, item.StatusCode);
            Assert.AreSame(options.Properties, item.Properties);
        }

        [TestMethod]
        public void PropertiesIsNotNull()
        {
            HttpResponse item = new HttpResponse(new HttpResponse.CreateOptions
            {
                Properties = null
            });

            Assert.IsNotNull(item.Properties);
        }

        [TestMethod]
        public void EndDateTimeIsInUtcFormat()
        {
            HttpResponse item = new HttpResponse(new HttpResponse.CreateOptions());

            Assert.AreEqual(DateTimeKind.Utc, item.EndDateTime.Kind);
        }

        [TestMethod]
        public void EndDateTimeIsInThePast()
        {
            HttpResponse item = new HttpResponse(new HttpResponse.CreateOptions());

            Assert.IsTrue(item.EndDateTime < DateTime.UtcNow);
        }

        [TestMethod]
        public void EndDateTimeHasValue()
        {
            HttpResponse item = new HttpResponse(new HttpResponse.CreateOptions());

            Assert.IsTrue(item.EndDateTime.Year > default(DateTime).Year);
        }

        [TestMethod]
        public void CloneCopiesAllTheProperties()
        {
            HttpResponse item = CommonTestHelpers.Factory.CreateHttpResponse();

            HttpResponse clone = item.Clone();

            Assert.AreEqual(JsonSerializer.Serialize(item), JsonSerializer.Serialize(clone));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetPropertiesThrowsExceptionForNullArgument()
        {
            HttpResponse item = new HttpResponse(new HttpResponse.CreateOptions());

            item.SetProperties(null);
        }

        [TestMethod]
        [DataRow(-100)]
        [DataRow(200)]
        [DataRow(404)]
        [DataRow(500)]
        public void SetStatusCodeUpdatesTheProperty(int statusCode)
        {
            HttpResponse item = new HttpResponse(new HttpResponse.CreateOptions());

            item.SetStatusCode(statusCode);

            Assert.AreEqual(statusCode, item.StatusCode);
        }
    }
}
