using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.RequestLogsListener;
using KissLog.RestClient;
using KissLog.RestClient.Api;
using KissLog.RestClient.Models;
using KissLog.RestClient.Requests.CreateRequestLog;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KissLog.CloudListeners.Tests.RequestLogsListener
{
    [TestClass]
    public class RequestLogsApiListenerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionWhenApplicationIsNull()
        {
            var listener = new RequestLogsApiListener(null);
        }

        [TestMethod]
        public void ApiUrlHasADefaultValue()
        {
            var application = new Application("organizationId", "applicationId");
            var listener = new RequestLogsApiListener(application);

            Assert.AreEqual(Constants.KissLogApiUrl, listener.ApiUrl);
        }

        [TestMethod]
        public void UseAsyncHasADefaultValue()
        {
            var application = new Application("organizationId", "applicationId");
            var listener = new RequestLogsApiListener(application);

            Assert.IsTrue(listener.UseAsync);
        }

        [TestMethod]
        public void IgnoreSslCertificateHasADefaultValue()
        {
            var application = new Application("organizationId", "applicationId");
            var listener = new RequestLogsApiListener(application);

            Assert.IsFalse(listener.IgnoreSslCertificate);
        }

        [TestMethod]
        public void OptionsIsNotNull()
        {
            Assert.IsNotNull(RequestLogsApiListener.Options);
        }

        [TestMethod]
        public void ObfuscationServiceIsNotNull()
        {
            var application = new Application("organizationId", "applicationId");
            var listener = new RequestLogsApiListener(application);

            Assert.IsNotNull(listener.ObfuscationService);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void ValidatePropertiesReturnsFalseForNullOrganizationId(string organizationId)
        {
            var application = new Application(organizationId, "applicationId");
            var listener = new RequestLogsApiListener(application);

            bool isValid = listener.ValidateProperties();

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void ValidatePropertiesReturnsFalseForNullApplicationId(string applicationId)
        {
            var application = new Application("organizationId", applicationId);
            var listener = new RequestLogsApiListener(application);

            bool isValid = listener.ValidateProperties();

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void ValidatePropertiesReturnsFalseForNullApiUrl(string apiUrl)
        {
            var application = new Application("organizationId", "applicationId");
            var listener = new RequestLogsApiListener(application)
            {
                ApiUrl = apiUrl
            };

            bool isValid = listener.ValidateProperties();

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        [DataRow("my-url")]
        [DataRow("my-url.com")]
        [DataRow("//my-url.com")]
        [DataRow("ftp://my-url.com")]
        public void ValidatePropertiesReturnsFalseForRelativeApiUrl(string apiUrl)
        {
            var application = new Application("organizationId", "applicationId");
            var listener = new RequestLogsApiListener(application)
            {
                ApiUrl = apiUrl
            };

            bool isValid = listener.ValidateProperties();

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        [DataRow("http://my-url/path")]
        [DataRow("http://my-url.com")]
        [DataRow("https://my-url.com")]
        [DataRow("http://my-url/path")]
        public void ValidatePropertiesReturnsTrueForValidApiUrl(string apiUrl)
        {
            var application = new Application("organizationId", "applicationId");
            var listener = new RequestLogsApiListener(application)
            {
                ApiUrl = apiUrl
            };

            bool isValid = listener.ValidateProperties();

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void UseAsyncIsEvaluated(bool useAsync)
        {
            bool createRequestLog = false;
            bool createRequestLogAsync = false;

            var kisslogApi = new Mock<IPublicApi>();
            kisslogApi.Setup(p => p.CreateRequestLog(It.IsAny<CreateRequestLogRequest>(), It.IsAny<IEnumerable<File>>())).Callback(() =>
            {
                createRequestLog = true;
            })
            .Returns(new ApiResult<RequestLog>());

            kisslogApi.Setup(p => p.CreateRequestLogAsync(It.IsAny<CreateRequestLogRequest>(), It.IsAny<IEnumerable<File>>())).Callback(() =>
            {
                createRequestLogAsync = true;
            })
            .Returns(Task.FromResult(new ApiResult<RequestLog>()));

            var listener = new RequestLogsApiListener(new Application("organizationId", "applicationId"), kisslogApi.Object)
            {
                UseAsync = useAsync
            };

            FlushLogArgs flushLogArgs = CommonTestHelpers.Factory.CreateFlushLogArgs();
            listener.OnFlush(flushLogArgs);

            if(useAsync == true)
            {
                Assert.IsFalse(createRequestLog);
                Assert.IsTrue(createRequestLogAsync);
            }
            else
            {
                Assert.IsTrue(createRequestLog);
                Assert.IsFalse(createRequestLogAsync);
            }
        }

        [TestMethod]
        public void NullObfuscationServiceDoesNotThrowException()
        {
            var kisslogApi = new Mock<IPublicApi>();
            kisslogApi.Setup(p => p.CreateRequestLog(It.IsAny<CreateRequestLogRequest>(), It.IsAny<IEnumerable<File>>()))
                .Returns(new ApiResult<RequestLog>());

            var listener = new RequestLogsApiListener(new Application("organizationId", "applicationId"), kisslogApi.Object)
            {
                UseAsync = false,
                ObfuscationService = null
            };

            FlushLogArgs flushLogArgs = CommonTestHelpers.Factory.CreateFlushLogArgs();
            listener.OnFlush(flushLogArgs);
        }
    }
}
