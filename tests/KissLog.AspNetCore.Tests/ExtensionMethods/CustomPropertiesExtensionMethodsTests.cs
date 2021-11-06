using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KissLog.AspNetCore.Tests
{
    [TestClass]
    public class CustomPropertiesExtensionMethodsTests
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
        public void AddString()
        {
            Logger kisslogger = new Logger();

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(kisslogger)
            };

            ILogger logger = new LoggerAdapter(options);

            logger.AddCustomProperty("string", "string-value");

            Assert.AreEqual("string", kisslogger.DataContainer.LoggerProperties.CustomProperties[0].Key);
            Assert.AreEqual("string-value", kisslogger.DataContainer.LoggerProperties.CustomProperties[0].Value);
        }

        [TestMethod]
        public void AddInteger()
        {
            Logger kisslogger = new Logger();

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(kisslogger)
            };

            ILogger logger = new LoggerAdapter(options);

            logger.AddCustomProperty("integer", 100);

            Assert.AreEqual("integer", kisslogger.DataContainer.LoggerProperties.CustomProperties[0].Key);
            Assert.AreEqual(100, kisslogger.DataContainer.LoggerProperties.CustomProperties[0].Value);
        }

        [TestMethod]
        public void AddDouble()
        {
            Logger kisslogger = new Logger();

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(kisslogger)
            };

            ILogger logger = new LoggerAdapter(options);

            logger.AddCustomProperty("double", 100.5D);

            Assert.AreEqual("double", kisslogger.DataContainer.LoggerProperties.CustomProperties[0].Key);
            Assert.AreEqual(100.5D, kisslogger.DataContainer.LoggerProperties.CustomProperties[0].Value);
        }

        [TestMethod]
        public void AddDecimal()
        {
            Logger kisslogger = new Logger();

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(kisslogger)
            };

            ILogger logger = new LoggerAdapter(options);

            logger.AddCustomProperty("decimal", 100.5M);

            Assert.AreEqual("decimal", kisslogger.DataContainer.LoggerProperties.CustomProperties[0].Key);
            Assert.AreEqual(100.5M, kisslogger.DataContainer.LoggerProperties.CustomProperties[0].Value);
        }

        [TestMethod]
        public void AddBoolean()
        {
            Logger kisslogger = new Logger();

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(kisslogger)
            };

            ILogger logger = new LoggerAdapter(options);

            logger.AddCustomProperty("boolean", true);

            Assert.AreEqual("boolean", kisslogger.DataContainer.LoggerProperties.CustomProperties[0].Key);
            Assert.AreEqual(true, kisslogger.DataContainer.LoggerProperties.CustomProperties[0].Value);
        }

        [TestMethod]
        public void AddDateTime()
        {
            Logger kisslogger = new Logger();

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(kisslogger)
            };

            ILogger logger = new LoggerAdapter(options);

            DateTime value = DateTime.UtcNow;
            logger.AddCustomProperty("dateTime", value);

            Assert.AreEqual("dateTime", kisslogger.DataContainer.LoggerProperties.CustomProperties[0].Key);
            Assert.AreEqual(value, kisslogger.DataContainer.LoggerProperties.CustomProperties[0].Value);
        }

        [TestMethod]
        public void AddGuid()
        {
            Logger kisslogger = new Logger();

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(kisslogger)
            };

            ILogger logger = new LoggerAdapter(options);

            Guid value = Guid.NewGuid();
            logger.AddCustomProperty("guid", value);

            Assert.AreEqual("guid", kisslogger.DataContainer.LoggerProperties.CustomProperties[0].Key);
            Assert.AreEqual(value, kisslogger.DataContainer.LoggerProperties.CustomProperties[0].Value);
        }
    }
}
