using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KissLog.Tests.ExtensionMethods
{
    [TestClass]
    public class CustomPropertiesExtensionMethodsTests
    {
        [TestMethod]
        public void NullLoggerDoesNotThrowException()
        {
            Type t = typeof(CustomPropertiesExtensionMethods);
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
            Logger logger = new Logger();

            logger.AddCustomProperty("string", "string-value");

            Assert.AreEqual("string", logger.DataContainer.LoggerProperties.CustomProperties[0].Key);
            Assert.AreEqual("string-value", logger.DataContainer.LoggerProperties.CustomProperties[0].Value);
        }

        [TestMethod]
        public void AddInteger()
        {
            Logger logger = new Logger();

            logger.AddCustomProperty("integer", 100);

            Assert.AreEqual("integer", logger.DataContainer.LoggerProperties.CustomProperties[0].Key);
            Assert.AreEqual(100, logger.DataContainer.LoggerProperties.CustomProperties[0].Value);
        }

        [TestMethod]
        public void AddDouble()
        {
            Logger logger = new Logger();

            logger.AddCustomProperty("double", 100.5D);

            Assert.AreEqual("double", logger.DataContainer.LoggerProperties.CustomProperties[0].Key);
            Assert.AreEqual(100.5D, logger.DataContainer.LoggerProperties.CustomProperties[0].Value);
        }

        [TestMethod]
        public void AddDecimal()
        {
            Logger logger = new Logger();

            logger.AddCustomProperty("decimal", 100.5M);

            Assert.AreEqual("decimal", logger.DataContainer.LoggerProperties.CustomProperties[0].Key);
            Assert.AreEqual(100.5M, logger.DataContainer.LoggerProperties.CustomProperties[0].Value);
        }

        [TestMethod]
        public void AddBoolean()
        {
            Logger logger = new Logger();

            logger.AddCustomProperty("boolean", true);

            Assert.AreEqual("boolean", logger.DataContainer.LoggerProperties.CustomProperties[0].Key);
            Assert.AreEqual(true, logger.DataContainer.LoggerProperties.CustomProperties[0].Value);
        }

        [TestMethod]
        public void AddDateTime()
        {
            Logger logger = new Logger();

            DateTime value = DateTime.UtcNow;
            logger.AddCustomProperty("dateTime", value);

            Assert.AreEqual("dateTime", logger.DataContainer.LoggerProperties.CustomProperties[0].Key);
            Assert.AreEqual(value, logger.DataContainer.LoggerProperties.CustomProperties[0].Value);
        }

        [TestMethod]
        public void AddGuid()
        {
            Logger logger = new Logger();

            Guid value = Guid.NewGuid();
            logger.AddCustomProperty("guid", value);

            Assert.AreEqual("guid", logger.DataContainer.LoggerProperties.CustomProperties[0].Key);
            Assert.AreEqual(value, logger.DataContainer.LoggerProperties.CustomProperties[0].Value);
        }
    }
}
