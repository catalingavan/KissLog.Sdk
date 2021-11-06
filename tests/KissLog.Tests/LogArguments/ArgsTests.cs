using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog.Tests.LogArguments
{
    [TestClass]
    public class ArgsTests
    {
        [TestMethod]
        public void NullArgsIsIgnored()
        {
            Args args = null;

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, args);

            int count = logger.DataContainer.LogMessages.Count();

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void EmptyArgsIsIgnored()
        {
            var args = new Args();

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, args);

            int count = logger.DataContainer.LogMessages.Count();

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void EmptyStringArgsIsIgnored()
        {
            var args = new Args(string.Empty);

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, args);

            int count = logger.DataContainer.LogMessages.Count();

            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void StringArgsIsLogged()
        {
            string value = $"String {Guid.NewGuid()}";

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, new Args(value));

            IEnumerable<LogMessage> logMessages = logger.DataContainer.LogMessages;

            bool exists = logMessages.Any(p => p.Message.Contains(value));

            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void ExceptionArgsIsLogged()
        {
            string value = $"Exception {Guid.NewGuid()}";

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, new Args(new Exception(value)));

            IEnumerable<LogMessage> logMessages = logger.DataContainer.LogMessages;

            bool exists = logMessages.Any(p => p.Message.Contains(value));

            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void NumeralArgsIsLogged()
        {
            int value = 100000;

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, new Args(value));

            IEnumerable<LogMessage> logMessages = logger.DataContainer.LogMessages;

            bool exists = logMessages.Any(p => p.Message.Contains(value.ToString()));

            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void ObjectArgsIsLogged()
        {
            var car = Car.Dacia;

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, new Args(car));

            IEnumerable<LogMessage> logMessages = logger.DataContainer.LogMessages;

            bool exists = logMessages.Any(p => p.Message.Contains(car.Make) && p.Message.Contains(car.Model));

            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void MixedArgsIsLogged()
        {
            string stringArg = $"String value {Guid.NewGuid()}";
            int numeralArg = 100000;
            Exception exArg = new Exception($"Exception {Guid.NewGuid()}");
            Car car = Car.Dacia;

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, new Args(stringArg, numeralArg, exArg, car));

            IEnumerable<LogMessage> logMessages = logger.DataContainer.LogMessages;

            bool stringExists = logMessages.Any(p => p.Message.Contains(stringArg));
            bool numeralExists = logMessages.Any(p => p.Message.Contains(numeralArg.ToString()));
            bool exceptionExists = logMessages.Any(p => p.Message.Contains(exArg.ToString()));
            bool objectExists = logMessages.Any(p => p.Message.Contains(car.Make) && p.Message.Contains(car.Model));

            Assert.IsTrue(stringExists);
            Assert.IsTrue(numeralExists);
            Assert.IsTrue(exceptionExists);
            Assert.IsTrue(objectExists);
        }
    }
}
