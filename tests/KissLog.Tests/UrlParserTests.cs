using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KissLog.Tests
{
    [TestClass]
    public class UrlParserTests
    {
        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("/")]
        [DataRow("/my/path")]
        [DataRow("http://my-application")]
        [DataRow("http://my-application/path")]
        public void ParsedUrlIsAlwaysAbsolute(string input)
        {
            Uri result = UrlParser.GenerateUri(input);

            Assert.IsTrue(result.IsAbsoluteUri);
        }

        [TestMethod]
        [DataRow(null, "http://application/")]
        [DataRow("", "http://application/")]
        [DataRow(" ", "http://application/")]
        [DataRow("/", "http://application/")]
        [DataRow("///", "http://application/")]
        [DataRow("*#-", "http://application/")]
        [DataRow("http://my-application/", "http://my-application/")]
        [DataRow("https://my-application///", "https://my-application/")]
        [DataRow("//path/to//Resource/", "http://application/path/to/Resource")]
        [DataRow("http://my-application//path?q=1&r=2", "http://my-application/path?q=1&r=2")]
        [DataRow("//Path/to//resource//?q=1&r=2", "http://application/Path/to/resource?q=1&r=2")]
        public void SpecialCharacthersAreHandled(string input, string expectedValue)
        {
            Uri result = UrlParser.GenerateUri(input);

            Assert.AreEqual(expectedValue, result.ToString());
        }
    }
}
