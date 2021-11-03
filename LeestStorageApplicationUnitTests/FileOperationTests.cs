using System;
using System.IO;
using System.IO.Abstractions;
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
        public async Task FilefromByteThrowsExceptionOnNullByteArrayTest()
        {
            byte[] file = null;

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await FileOperation.FileFromByteArray("_]a;';l#WrongDirectory", file), "A byte array was returned without a proper filePath");
        }

        [TestMethod]
        public async Task FileToByteThrowsExceptionOnWrongFilePathTest()
        {
            await Assert.ThrowsExceptionAsync<FileNotFoundException>(async() => await FileOperation.FileToByteArray("_]a;';l#WrongDirectory"), "A byte array was returned without a proper filePath");
        }
    }
}
