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

            //We clean up the test files
            //We do not use the FileOperation.DeleteFileOrDirectory() method to prevent relying on something that might break.
            File.Delete(downloadLocation);
        }

        [TestMethod]
        public void DeleteFileTest()
        {
            string downloadLocation = new KnownFolder(KnownFolderType.Downloads).Path + @"\DeleteFileTest1234.text";

            using (StreamWriter sw = new StreamWriter(downloadLocation))
            {
                sw.Write("This is a test");
            }

            FileOperation.DeleteFileOrDirectory(downloadLocation);

            Assert.IsFalse(File.Exists(downloadLocation));
        }

        [TestMethod]
        public void DeleteDirectoryTest()
        {
            string directoryDownloadLocation = new KnownFolder(KnownFolderType.Downloads).Path + @"\DeleteDirectoryTest";
            string fileDownloadLocation = directoryDownloadLocation + @"\DeleteDirectoryTest1234.text";

            Directory.CreateDirectory(directoryDownloadLocation);
            using (StreamWriter sw = new StreamWriter(fileDownloadLocation))
            {
               sw.Write("This is a test");
            }

            FileOperation.DeleteFileOrDirectory(directoryDownloadLocation);

            bool result = (File.Exists(fileDownloadLocation) || Directory.Exists(directoryDownloadLocation));
            
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ReturnFilesFromDirectoryTest()
        {
            string directoryDownloadLocation = new KnownFolder(KnownFolderType.Downloads).Path + @"\ReturnFilesFromDirectoryTest";
            string file1DownloadLocation = directoryDownloadLocation + @"\ReturnFilesFromDirectoryTest1234.text";
            string file2DownloadLocation = directoryDownloadLocation + @"\ReturnFilesFromDirectoryTest5678.text";

            Directory.CreateDirectory(directoryDownloadLocation);
            using (StreamWriter sw1 = new StreamWriter(file1DownloadLocation) )
            using (StreamWriter sw2 = new StreamWriter(file2DownloadLocation))
            {
                sw1.Write("This is a test");
                sw2.Write("This is also a test");
            }


            string[] fileLocations = FileOperation.ReturnFilesFromDirectory(directoryDownloadLocation);

            bool result = (fileLocations[0] == file1DownloadLocation && fileLocations[1] == file2DownloadLocation);

            Assert.IsTrue(result);

            //We clean up the test files
            //We do not use the FileOperation.DeleteFileOrDirectory() method to prevent relying on something that might break.
            File.Delete(file1DownloadLocation);
            File.Delete(file2DownloadLocation);
            Directory.Delete(directoryDownloadLocation);
        }



        [TestMethod]
        public async Task FileToAndFromByteArrayTest()
        {
            string downloadLocation = new KnownFolder(KnownFolderType.Downloads).Path + @"\FileToAndFromByteArrayTest1234.text";
            using (StreamWriter sw = new StreamWriter(downloadLocation))
            {
                sw.Write("This is a test");
                sw.Write("This is the second line of the test");
            }

            var byteArray = await FileOperation.FileToByteArray(downloadLocation);
            FileOperation.DeleteFileOrDirectory(downloadLocation);
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

            //We clean up the test files
            //We do not use the FileOperation.DeleteFileOrDirectory() method to prevent relying on something that might break.
            File.Delete(downloadLocation);
        }
    }
}
