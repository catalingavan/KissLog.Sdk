using KissLog.CloudListeners.RequestLogsListener;
using KissLog.RestClient;
using KissLog.RestClient.Api;
using KissLog.RestClient.Models;
using KissLog.RestClient.Requests.CreateRequestLog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.CloudListeners.Tests.RequestLogsListener
{
    [TestClass]
    public class FlusherTests
    {
        [TestMethod]
        public void ChangesFlushLogArgsFiles()
        {
            Logger logger = new Logger();
            logger.LogAsFile("Content 1", "File1.txt");
            logger.LogAsFile("Content 2", "File2.txt");

            FlushLogArgs flushArgs = FlushLogArgsFactory.Create(new[] { logger });
            IEnumerable<LoggedFile> files = flushArgs.Files;
            
            CreateRequestLogRequest request = PayloadFactory.Create(flushArgs);

            var kisslogApi = new Mock<IPublicApi>();
            kisslogApi.Setup(p => p.CreateRequestLog(It.IsAny<CreateRequestLogRequest>(), It.IsAny<IEnumerable<File>>()))
                .Returns(new ApiResult<RequestLog>());

            Flusher.FlushAsync(new FlushOptions { UseAsync = false }, kisslogApi.Object, flushArgs, request).ConfigureAwait(false);

            Assert.AreNotSame(files, flushArgs.Files);
            Assert.AreEqual(files.Count(), flushArgs.Files.Count());

            logger.Reset();
        }

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void CreatesACopyOfTheFilesAndDeletesThem(bool apiThrowsException)
        {
            Logger logger = new Logger();
            logger.LogAsFile("Content 1", "File1.txt");
            logger.LogAsFile("Content 2", "File2.txt");

            FlushLogArgs flushArgs = FlushLogArgsFactory.Create(new[] { logger });
            CreateRequestLogRequest request = PayloadFactory.Create(flushArgs);

            var kisslogApi = new Mock<IPublicApi>();
            kisslogApi.Setup(p => p.CreateRequestLog(It.IsAny<CreateRequestLogRequest>(), It.IsAny<IEnumerable<File>>())).Callback(() =>
            {
                if (apiThrowsException)
                    throw new Exception();
            })
            .Returns(new ApiResult<RequestLog>());

            Flusher.FlushAsync(new FlushOptions { UseAsync = false }, kisslogApi.Object, flushArgs, request).ConfigureAwait(false);

            IEnumerable<LoggedFile> flushArgsFiles = flushArgs.Files;
            IEnumerable<LoggedFile> loggerFiles = logger.DataContainer.FilesContainer.GetLoggedFiles();

            foreach(var file in flushArgsFiles)
            {
                Assert.IsFalse(System.IO.File.Exists(file.FilePath));
            }

            foreach (var file in loggerFiles)
            {
                Assert.IsTrue(System.IO.File.Exists(file.FilePath));
            }

            logger.Reset();
        }

        [TestMethod]
        public void OnExceptionIsExecutedWhenApiResultHasException()
        {
            Logger logger = new Logger();

            ExceptionArgs exceptionArgs = null;

            FlushLogArgs flushArgs = FlushLogArgsFactory.Create(new[] { logger });
            CreateRequestLogRequest request = PayloadFactory.Create(flushArgs);

            var kisslogApi = new Mock<IPublicApi>();
            kisslogApi.Setup(p => p.CreateRequestLog(It.IsAny<CreateRequestLogRequest>(), It.IsAny<IEnumerable<File>>()))
                .Returns(new ApiResult<RequestLog>
                {
                    Exception = new ApiException
                    {
                        ErrorMessage = $"Error {Guid.NewGuid()}"
                    }
                });

            FlushOptions options = new FlushOptions
            {
                UseAsync = false,
                OnException = (ExceptionArgs args) =>
                {
                    exceptionArgs = args;
                }
            };

            Flusher.FlushAsync(options, kisslogApi.Object, flushArgs, request).ConfigureAwait(false);

            Assert.IsNotNull(exceptionArgs);
        }

        [TestMethod]
        public void ExceptionArgsContainsAReferenceToApiResult()
        {
            Logger logger = new Logger();

            ApiResult<RequestLog> result = new ApiResult<RequestLog>
            {
                Exception = new ApiException
                {
                    ErrorMessage = $"Error {Guid.NewGuid()}"
                }
            };
            ExceptionArgs exceptionArgs = null;

            FlushLogArgs flushArgs = FlushLogArgsFactory.Create(new[] { logger });
            CreateRequestLogRequest request = PayloadFactory.Create(flushArgs);

            var kisslogApi = new Mock<IPublicApi>();
            kisslogApi.Setup(p => p.CreateRequestLog(It.IsAny<CreateRequestLogRequest>(), It.IsAny<IEnumerable<File>>()))
                .Returns(result);

            FlushOptions options = new FlushOptions
            {
                UseAsync = false,
                OnException = (ExceptionArgs args) =>
                {
                    exceptionArgs = args;
                }
            };

            Flusher.FlushAsync(options, kisslogApi.Object, flushArgs, request).ConfigureAwait(false);

            Assert.AreSame(result, exceptionArgs.ApiResult);
        }

        [TestMethod]
        public void ExceptionArgsContainsAReferenceToFlushLogArgs()
        {
            Logger logger = new Logger();

            ApiResult<RequestLog> result = new ApiResult<RequestLog>
            {
                Exception = new ApiException
                {
                    ErrorMessage = $"Error {Guid.NewGuid()}"
                }
            };
            ExceptionArgs exceptionArgs = null;

            FlushLogArgs flushArgs = FlushLogArgsFactory.Create(new[] { logger });
            CreateRequestLogRequest request = PayloadFactory.Create(flushArgs);

            var kisslogApi = new Mock<IPublicApi>();
            kisslogApi.Setup(p => p.CreateRequestLog(It.IsAny<CreateRequestLogRequest>(), It.IsAny<IEnumerable<File>>()))
                .Returns(new ApiResult<RequestLog>
                {
                    Exception = new ApiException
                    {
                        ErrorMessage = $"Error {Guid.NewGuid()}"
                    }
                });

            FlushOptions options = new FlushOptions
            {
                UseAsync = false,
                OnException = (ExceptionArgs args) =>
                {
                    exceptionArgs = args;
                }
            };

            Flusher.FlushAsync(options, kisslogApi.Object, flushArgs, request).ConfigureAwait(false);

            Assert.AreSame(flushArgs, exceptionArgs.FlushArgs);
        }
    }
}
