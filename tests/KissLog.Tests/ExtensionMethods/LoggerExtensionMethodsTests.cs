using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KissLog.Tests.ExtensionMethods
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
            Logger logger = new Logger();

            logger.SetStatusCode(statusCode);

            Assert.AreEqual(statusCode, logger.DataContainer.LoggerProperties.ExplicitStatusCode.Value);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void LogResponseBodyUpdatesTheLoggerProperties(bool value)
        {
            Logger logger = new Logger();

            logger.LogResponseBody(value);

            Assert.AreEqual(value, logger.DataContainer.LoggerProperties.ExplicitLogResponseBody.Value);
        }

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void AutoFlushReturnsTrueIfLoggerIsManagedByHttpRequestIsTrue(bool isManagedByHttpRequest)
        {
            Logger logger = new Logger();

            logger.DataContainer.LoggerProperties.IsManagedByHttpRequest = isManagedByHttpRequest;

            Assert.AreEqual(isManagedByHttpRequest, logger.AutoFlush());
        }
    }
}
