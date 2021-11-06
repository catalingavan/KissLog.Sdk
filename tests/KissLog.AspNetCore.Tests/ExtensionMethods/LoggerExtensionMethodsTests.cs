using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KissLog.AspNetCore.Tests
{
    [TestClass]
    public class LoggerExtensionMethodsTests
    {
        [TestMethod]
        public void NullLoggerDoesNotThrowException()
        {
            Type t = typeof(LoggerExtensionMethods);
            List<MethodInfo> methods = t.GetMethods(BindingFlags.Public | BindingFlags.Static).ToList();

            foreach (MethodInfo method in methods)
            {
                object[] parameters = method.GetParameters().Select(p => p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null).ToArray();
                method.Invoke(null, parameters);
            }
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(200)]
        [DataRow(404)]
        [DataRow(500)]
        public void SetStatusCodeUpdatesTheLoggerProperties(int statusCode)
        {
            Logger kisslogger = new Logger();

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(kisslogger)
            };

            ILogger logger = new LoggerAdapter(options);

            logger.SetStatusCode(statusCode);

            Assert.AreEqual(statusCode, kisslogger.DataContainer.LoggerProperties.ExplicitStatusCode.Value);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void LogResponseBodyUpdatesTheLoggerProperties(bool value)
        {
            Logger kisslogger = new Logger();

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(kisslogger)
            };

            ILogger logger = new LoggerAdapter(options);

            logger.LogResponseBody(value);

            Assert.AreEqual(value, kisslogger.DataContainer.LoggerProperties.ExplicitLogResponseBody.Value);
        }

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void AutoFlushReturnsTrueIfLoggerIsManagedByHttpRequestIsTrue(bool isManagedByHttpRequest)
        {
            Logger kisslogger = new Logger();
            kisslogger.DataContainer.LoggerProperties.IsManagedByHttpRequest = isManagedByHttpRequest;

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(kisslogger)
            };

            ILogger logger = new LoggerAdapter(options);

            Assert.AreEqual(isManagedByHttpRequest, logger.AutoFlush());
        }
    }
}
