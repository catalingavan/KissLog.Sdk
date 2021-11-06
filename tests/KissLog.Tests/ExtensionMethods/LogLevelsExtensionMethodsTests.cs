using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KissLog.Tests.ExtensionMethods
{
    [TestClass]
    public class LogLevelsExtensionMethodsTests
    {
        [TestMethod]
        public void NullLoggerDoesNotThrowException()
        {
            Type t = typeof(LogLevelsExtensionMethods);
            List<MethodInfo> methods = t.GetMethods(BindingFlags.Public | BindingFlags.Static).ToList();

            foreach (MethodInfo method in methods)
            {
                object[] parameters = method.GetParameters().Select(p => p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null).ToArray();
                method.Invoke(null, parameters);
            }
        }

        [TestMethod]
        public void ExtensionMethodsGeneratesCorrectOutput()
        {
            Type t = typeof(LogLevelsExtensionMethods);
            List<MethodInfo> methods = t.GetMethods(BindingFlags.Public | BindingFlags.Static).ToList();

            foreach(MethodInfo method in methods)
            {
                IKLogger logger = new Logger();

                LogLevel logLevel = GetLogLevelFromMethodName(method.Name);
                Type argumentType = method.GetParameters()[1].ParameterType;
                object[] parameters = method.GetParameters().Select(p => p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null).ToArray();

                string expectedMessage = null;

                if(argumentType == typeof(string))
                {
                    string stringParameter = $"Log message: {Guid.NewGuid()}";
                    expectedMessage = GetExpectedMessage(stringParameter);

                    parameters[1] = stringParameter;
                }
                else if(argumentType == typeof(Exception))
                {
                    Exception exceptionParameter = new Exception($"Exception {Guid.NewGuid()}");
                    expectedMessage = GetExpectedMessage(exceptionParameter);

                    parameters[1] = exceptionParameter;
                }
                else if(argumentType == typeof(Args))
                {
                    Args argsParameter = new Args($"String value {Guid.NewGuid()}", 100000);
                    expectedMessage = GetExpectedMessage(argsParameter);

                    parameters[1] = argsParameter;
                }
                else if(argumentType == typeof(object))
                {
                    object objectParameter = Car.Dacia;
                    expectedMessage = GetExpectedMessage(objectParameter);

                    parameters[1] = objectParameter;
                }

                parameters[0] = logger;

                method.Invoke(null, parameters);

                LogMessage logMessage = (logger as Logger).DataContainer.LogMessages.FirstOrDefault();

                Assert.IsNotNull(logMessage);
                Assert.AreEqual(logLevel, logMessage.LogLevel);
                Assert.AreEqual(expectedMessage, logMessage.Message);
            }
        }

        public LogLevel GetLogLevelFromMethodName(string name)
        {
            switch(name)
            {
                case "Trace":
                    return LogLevel.Trace;

                case "Debug":
                    return LogLevel.Debug;

                case "Info":
                    return LogLevel.Information;

                case "Warn":
                    return LogLevel.Warning;

                case "Error":
                    return LogLevel.Error;

                case "Critical":
                    return LogLevel.Critical;

                default:
                    throw new ArgumentException($"Method {name} does not have an equivalent LogLevel");
            }
        }

        private string GetExpectedMessage(string stringParameter)
        {
            Logger logger = new Logger();
            logger.Trace(stringParameter);

            return logger.DataContainer.LogMessages.First().Message;
        }

        private string GetExpectedMessage(Exception exceptionParameter)
        {
            Logger logger = new Logger();
            logger.Trace(exceptionParameter);

            return logger.DataContainer.LogMessages.First().Message;
        }

        private string GetExpectedMessage(Args argsParameter)
        {
            Logger logger = new Logger();
            logger.Trace(argsParameter);

            return logger.DataContainer.LogMessages.First().Message;
        }

        private string GetExpectedMessage(object objectParameter)
        {
            Logger logger = new Logger();
            logger.Trace(objectParameter);

            return logger.DataContainer.LogMessages.First().Message;
        }
    }
}
