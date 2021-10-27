using System;
using System.Text.RegularExpressions;
using LeestStorageApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeestStorageApplicationUnitTests
{

    [TestClass]
    public class DirectoryFileTests
    {

        [TestMethod]
        public void DetailInfoEmpty()
        {
            DirectoryFile directoryFile = new("Test", "", DateTime.Now);
            Assert.AreEqual("Type: File", directoryFile.DetailInfo, "DetailInfo remained empty.");
        }

        [TestMethod]
        public void DetailInfoNoType()
        {
            DirectoryFile directoryFile = new("Test", "Last Changed: " + DateTime.UtcNow.ToString(), DateTime.Now);
            bool result = directoryFile.DetailInfo.Contains("Type: File");
            Assert.IsTrue(result, "DetailInfo does not contain \'Type: File\'.");
        }

        [TestMethod]
        public void DetailInfoDoubleType()
        {
            DirectoryFile directoryFile = new("Test", "", DateTime.Now);
            int count = new Regex($"(Type: File)+").Matches(directoryFile.DetailInfo.ToString()).Count;
            Assert.AreEqual(1, count, "\'Type: File\' has been detected more than once in DetailInfo");
        }

    }
}
