using System;
using System.IO;
using System.Threading.Tasks;
using CommunicationObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Syroot.Windows.IO;

namespace LeestStorageApplicationUnitTests
{
    [TestClass]
    public class FileOperationTests
    {
        [TestMethod]
        public async Task FileToByteArrayTest()
        {
            string downloadLocation = new KnownFolder(KnownFolderType.Downloads).Path + @"\FileToByteArrayTest1234.text";
            using (StreamWriter sw = new StreamWriter(downloadLocation ))
            {
                sw.Write("This is a test");
                sw.Write("This is the second line of the test");
            }

            var byteArray = await FileOperation.FileToByteArray(downloadLocation);


            Assert.IsInstanceOfType(byteArray, typeof(byte[]), "FileToByteArray does not return a byte array"); 
        }

        [TestMethod]
        public async Task FileToAndFromByteArrayTest()
        {
            string downloadLocation = new KnownFolder(KnownFolderType.Downloads).Path + @"\FileToByteArrayTest1234.text";
            using (StreamWriter sw = new StreamWriter(downloadLocation))
            {
                sw.Write("This is a test");
                sw.Write("This is the second line of the test");
            }

            var byteArray = await FileOperation.FileToByteArray(downloadLocation);
            FileOperation.deleteFile(downloadLocation);
            await FileOperation.FileFromByteArray(downloadLocation, byteArray);

            string text1;
            string text2;

            using (StreamReader sw = new StreamReader(downloadLocation))
            {
                text1 = sw.ReadLine();
                text2 = sw.ReadLine();
            }

            bool textTest = (text1.Equals("This is a test") && text2.Equals("This is the second line of the test"));

            Assert.IsInstanceOfType(byteArray, typeof(byte[]), "FileToByteArray does not return a byte array");
        }
    }
}
