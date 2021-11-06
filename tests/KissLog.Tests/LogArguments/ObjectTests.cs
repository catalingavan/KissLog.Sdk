using KissLog.Json;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text.Json;

namespace KissLog.Tests.LogArguments
{
    [TestClass]
    public class ObjectTests
    {
        [TestMethod]
        public void NullObjectCreatesAMessage()
        {
            Car car = null;

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, car);

            int count = logger.DataContainer.LogMessages.Count();

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void ObjectCreatesAMessage()
        {
            var car = Car.Dacia;

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, car);

            int count = logger.DataContainer.LogMessages.Count();

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void ObjectIsLoggedAsJson()
        {
            var car = Car.Dacia;

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, car);

            string message = logger.DataContainer.LogMessages.First().Message;

            car = JsonSerializer.Deserialize<Car>(message);

            Assert.IsNotNull(car);
        }

        [TestMethod]
        public void NullObjectIsLoggedAsJson()
        {
            Car car = null;

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, car);

            string message = logger.DataContainer.LogMessages.First().Message;

            car = JsonSerializer.Deserialize<Car>(message);

            Assert.IsNull(car);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void WriteIndentedOption(bool writeIndented)
        {
            var car = Car.Dacia;

            Logger logger = new Logger();
            logger.Log(LogLevel.Trace, car, new JsonSerializeOptions { WriteIndented = writeIndented });

            string result = logger.DataContainer.LogMessages.First().Message;
            string expectedResult = JsonSerializer.Serialize(car, new JsonSerializerOptions { WriteIndented = writeIndented });

            Assert.AreEqual(expectedResult, result);
        }
    }
}
