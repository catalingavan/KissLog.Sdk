using KissLog.Formatters;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace KissLog.Tests.Formatters
{
    [TestClass]
    public class ExceptionFormatterTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FormatThrowsExceptionForNullLogger()
        {
            var formatter = new ExceptionFormatter();

            formatter.Format(new Exception(), null);
        }

        [TestMethod]
        public void FormatReturnsEmptyStringForNullException()
        {
            var formatter = new ExceptionFormatter();

            string result = formatter.Format(null, new Logger());

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void OnlyRootExceptionsAreStoredInLoggerDataContainer()
        {
            var ex1 = new Exception("Exception 1",
                new Exception("Exception 2")
            );

            var ex2 = new Exception("Exception 3",
                new Exception("Inner exception 4",
                    new Exception("Inner exception 5")
                )
            );

            Logger logger = new Logger();
            
            var formatter = new ExceptionFormatter();
            formatter.Format(ex1, logger);
            formatter.Format(ex2, logger);

            int count = logger.DataContainer.Exceptions.Count();

            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void SameExceptionInstanceIsNotLoggedTwiceByTheSameLogger()
        {
            var ex = new Exception($"Exception: {Guid.NewGuid()}");

            Logger logger = new Logger();

            var formatter = new ExceptionFormatter();
            formatter.Format(ex, logger);
            formatter.Format(ex, logger);

            int count = logger.DataContainer.Exceptions.Count();

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void SameExceptionInstanceIsLoggedByDifferentLoggers()
        {
            var ex = new Exception($"Exception: {Guid.NewGuid()}");

            Logger logger1 = new Logger();
            Logger logger2 = new Logger();

            var formatter = new ExceptionFormatter();
            formatter.Format(ex, logger1);
            formatter.Format(ex, logger2);

            int count1 = logger1.DataContainer.Exceptions.Count();
            int count2 = logger2.DataContainer.Exceptions.Count();

            Assert.AreEqual(1, count1);
            Assert.AreEqual(1, count2);
        }

        [TestMethod]
        public void AllExceptionsAreLogged()
        {
            string exception1 = $"Exception: {Guid.NewGuid()}";
            string exception2 = $"Exception 2: {Guid.NewGuid()}";
            string exception3 = $"Exception 3: {Guid.NewGuid()}";
            string exception4 = $"Exception 4: {Guid.NewGuid()}";

            var ex = new Exception(exception1,
                new Exception(exception2,
                    new Exception(exception3,
                        new Exception(exception4)
                    )
                )
            );

            var formatter = new ExceptionFormatter();
            string result = formatter.Format(ex, new Logger());

            Assert.IsTrue(result.Contains(exception1));
            Assert.IsTrue(result.Contains(exception2));
            Assert.IsTrue(result.Contains(exception3));
            Assert.IsTrue(result.Contains(exception4));
        }

        [TestMethod]
        public void SameExceptionMessageIsLoggedEachTime()
        {
            string message = $"Exception: {Guid.NewGuid()}";

            var ex1 = new Exception(message);
            var ex2 = new Exception(message);

            Logger logger = new Logger();
            
            var formatter = new ExceptionFormatter();
            formatter.Format(ex1, logger);
            formatter.Format(ex2, logger);

            int count = logger.DataContainer.Exceptions.Count();

            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void SameInnerExceptionDoesNotThrowException()
        {
            var innerEx = new Exception($"Inner exception: {Guid.NewGuid()}");

            var ex1 = new Exception("Exception 1", innerEx);
            var ex2 = new Exception("Exception 2", innerEx);

            Logger logger = new Logger();

            var formatter = new ExceptionFormatter();
            formatter.Format(ex1, logger);
            formatter.Format(ex2, logger);

            int count = logger.DataContainer.Exceptions.Count();

            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void InnerExceptionIsLoggedOnlyOnce()
        {
            var innerEx = new Exception($"Inner exception: {Guid.NewGuid()}");
            var ex = new Exception("Exception", innerEx);

            Logger logger = new Logger();

            var formatter = new ExceptionFormatter();
            formatter.Format(ex, logger);
            formatter.Format(innerEx, logger);

            int count = logger.DataContainer.Exceptions.Count();

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void AppendExceptionDetailsOptions()
        {
            CommonTestHelpers.ResetContext();

            KissLogConfiguration.Options.AppendExceptionDetails((Exception ex) =>
            {
                if(ex is NotImplementedException implementedException)
                {
                    return "We should implement this method";
                }

                return null;
            });

            var formatter = new ExceptionFormatter();
            string result = formatter.Format(new NotImplementedException(), new Logger());

            Assert.IsTrue(result.Contains("We should implement this method"));
        }
    }
}
