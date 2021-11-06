using KissLog.Json;
using KissLog.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KissLog.Tests.Json
{
    [TestClass]
    public class SystemTextJsonSerializerTests
    {
        [TestMethod]
        public void NullOptionsDoesNotThrowException()
        {
            IJsonSerializer serializer = new SystemTextJsonSerializer();
            Car car = Car.Dacia;

            string result = serializer.Serialize(car, null);

            Assert.IsTrue(!string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void NullObjectIsFormattedAsJson()
        {
            IJsonSerializer serializer = new SystemTextJsonSerializer();
            Car car = null;

            string result = serializer.Serialize(car, null);

            car = System.Text.Json.JsonSerializer.Deserialize<Car>(result);

            Assert.IsNull(car);
        }

        [TestMethod]
        public void ObjectIsFormattedAsJson()
        {
            IJsonSerializer serializer = new SystemTextJsonSerializer();
            Car car = Car.Dacia;

            string result = serializer.Serialize(car, null);

            car = System.Text.Json.JsonSerializer.Deserialize<Car>(result);

            Assert.IsNotNull(car);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void WriteIndentedOption(bool writeIndented)
        {
            IJsonSerializer serializer = new SystemTextJsonSerializer();
            Car car = Car.Dacia;

            string result = serializer.Serialize(car, new JsonSerializeOptions { WriteIndented = writeIndented });
            string expectedResult = System.Text.Json.JsonSerializer.Serialize(car, new System.Text.Json.JsonSerializerOptions { WriteIndented = writeIndented });

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void DeserializeWorks()
        {
            string json = System.Text.Json.JsonSerializer.Serialize(Car.Dacia);

            IJsonSerializer serializer = new SystemTextJsonSerializer();
            string result = serializer.Serialize(Car.Dacia);

            Assert.AreEqual(json, result);
        }

        [TestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow(" ", false)]
        [DataRow("{...}", false)]
        [DataRow("{ }", true)]
        [DataRow("[...]", false)]
        [DataRow("[]", true)]
        [DataRow("null", false)]
        public void IsJson(string strInput, bool expectedValue)
        {
            IJsonSerializer serializer = new SystemTextJsonSerializer();
            bool result = serializer.IsJson(strInput);

            Assert.AreEqual(expectedValue, result);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("{...}")]
        [DataRow("[...]")]
        [DataRow("null")]
        public void DeserializeAndFlattenDoesNotThrowExceptionForNullOrInvalidJson(string json)
        {
            IJsonSerializer serializer = new SystemTextJsonSerializer();
            List<KeyValuePair<string, object>> result = serializer.DeserializeAndFlatten(json).ToList();
        }

        [TestMethod]
        public void DeserializeAndFlattenWorks()
        {
            DirectoryInfo testDataDir = CommonTestHelpers.FindTestDataDirectory();

            IJsonSerializer serializer = new SystemTextJsonSerializer();

            foreach (var file in testDataDir.EnumerateFiles("*.json"))
            {
                string json = File.ReadAllText(file.FullName);
                List<KeyValuePair<string, object>> result = serializer.DeserializeAndFlatten(json).ToList();
            }
        }
    }
}
