using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.AspNetCore.Tests
{
    [TestClass]
    public class LoggerAdapterTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionForNullOptions()
        {
            ILogger adapter = new LoggerAdapter(null);
        }

        [TestMethod]
        public void DoesNotThrowExceptionWhenFormatterIsNull()
        {
            Logger logger = new Logger();

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(logger),
                Formatter = null
            };

            ILogger adapter = new LoggerAdapter(options);

            adapter.Log(Microsoft.Extensions.Logging.LogLevel.Debug, 10, new { }, null, (state, ex) => { return "Default formatter message"; });

            LogMessage message = logger.DataContainer.LogMessages.First();

            Assert.AreEqual("Default formatter message", message.Message);
        }

        [TestMethod]
        public void DoesNotThrowExceptionWhenFactoryIsNull()
        {
            var options = new LoggerOptions
            {
                Factory = null
            };

            ILogger adapter = new LoggerAdapter(options);

            adapter.Log(Microsoft.Extensions.Logging.LogLevel.Debug, 10, new { }, null, (state, ex) => { return "Default formatter message"; });
        }

        [TestMethod]
        public void CustomFormatterIsUsed()
        {
            Logger logger = new Logger();

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(logger),
                Formatter = (FormatterArgs args) => "Custom formatter message"
            };

            ILogger adapter = new LoggerAdapter(options);

            adapter.Log(Microsoft.Extensions.Logging.LogLevel.Debug, 10, new { }, null, (a, b) => { return "Default formatter message"; });

            LogMessage message = logger.DataContainer.LogMessages.First();

            Assert.AreEqual("Custom formatter message", message.Message);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        public void DefaultFormatterIsUsedWhenCustomFormatterReturnsNull(string value)
        {
            Logger logger = new Logger();

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(logger),
                Formatter = (FormatterArgs args) => value
            };

            ILogger adapter = new LoggerAdapter(options);

            adapter.Log(Microsoft.Extensions.Logging.LogLevel.Debug, 10, new { }, null, (state, ex) => { return "Default formatter message"; });

            LogMessage message = logger.DataContainer.LogMessages.First();

            Assert.AreEqual("Default formatter message", message.Message);
        }

        [TestMethod]
        [DataRow(Microsoft.Extensions.Logging.LogLevel.Trace, LogLevel.Trace)]
        [DataRow(Microsoft.Extensions.Logging.LogLevel.Debug, LogLevel.Debug)]
        [DataRow(Microsoft.Extensions.Logging.LogLevel.Information, LogLevel.Information)]
        [DataRow(Microsoft.Extensions.Logging.LogLevel.Warning, LogLevel.Warning)]
        [DataRow(Microsoft.Extensions.Logging.LogLevel.Error, LogLevel.Error)]
        [DataRow(Microsoft.Extensions.Logging.LogLevel.Critical, LogLevel.Critical)]
        public void LogLevelIsAssignedCorrectly(Microsoft.Extensions.Logging.LogLevel aspNetCoreLogLevel, LogLevel kissLogLogLevel)
        {
            Logger logger = new Logger();

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(logger)
            };

            ILogger adapter = new LoggerAdapter(options);

            adapter.Log(aspNetCoreLogLevel, 10, new { }, null, (state, ex) => { return "Default formatter message"; });

            LogMessage message = logger.DataContainer.LogMessages.First();

            Assert.AreEqual(kissLogLogLevel, message.LogLevel);
        }

        [TestMethod]
        public void BeginScopeDoesNothingIfNoActionsAreDefined()
        {
            Logger logger = new Logger();

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(logger),
                OnBeginScope = null,
                OnEndScope = null
            };

            ILogger adapter = new LoggerAdapter(options);

            using (adapter.BeginScope("Scope"))
            {
                adapter.LogInformation("Info message");
            }

            Assert.AreEqual(1, logger.DataContainer.LogMessages.Count());
            Assert.IsTrue(logger.DataContainer.LogMessages.ElementAt(0).Message == "Info message");
        }

        [TestMethod]
        public void BeginScopeInvokesOptionsOnBeginScope()
        {
            Logger logger = new Logger();

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(logger),
                OnBeginScope = (BeginScopeArgs args) =>
                {
                    string message = args.State?.ToString();
                    args.Logger.Trace(message);
                }
            };

            ILogger adapter = new LoggerAdapter(options);

            using(adapter.BeginScope("Scope"))
            {
                adapter.LogInformation("Info message");
            }

            Assert.AreEqual(2, logger.DataContainer.LogMessages.Count());
            Assert.IsTrue(logger.DataContainer.LogMessages.ElementAt(0).Message == "Scope");
            Assert.IsTrue(logger.DataContainer.LogMessages.ElementAt(1).Message == "Info message");
        }

        [TestMethod]
        public void EndScopeInvokesOptionsOnEndScope()
        {
            Logger logger = new Logger();

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(logger),
                OnBeginScope = (BeginScopeArgs args) =>
                {
                    string message = args.State?.ToString();
                    args.Logger.Trace(message);
                },
                OnEndScope = (EndScopeArgs args) =>
                {
                    args.Logger.Trace("Scope ended");
                }
            };

            ILogger adapter = new LoggerAdapter(options);

            using (adapter.BeginScope("Scope"))
            {
                adapter.LogInformation("Info message");
            }

            Assert.AreEqual(3, logger.DataContainer.LogMessages.Count());
            Assert.IsTrue(logger.DataContainer.LogMessages.ElementAt(0).Message == "Scope");
            Assert.IsTrue(logger.DataContainer.LogMessages.ElementAt(1).Message == "Info message");
            Assert.IsTrue(logger.DataContainer.LogMessages.ElementAt(2).Message == "Scope ended");
        }

        [TestMethod]
        public void ScopeDataDictionaryIsReferenced()
        {
            Logger logger = new Logger();

            string value = $"ScopeData-{Guid.NewGuid()}";
            IDictionary<string, object> scopeData = null;

            var options = new LoggerOptions
            {
                Factory = new KissLog.LoggerFactory(logger),
                OnBeginScope = (BeginScopeArgs args) =>
                {
                    args.ScopeData.Add("Key", value);
                },
                OnEndScope = (EndScopeArgs args) =>
                {
                    scopeData = args.ScopeData;
                }
            };

            ILogger adapter = new LoggerAdapter(options);

            using (adapter.BeginScope(null))
            {

            }

            Assert.IsNotNull(scopeData);
            Assert.AreEqual(value, scopeData["Key"]);
        }
    }
}
