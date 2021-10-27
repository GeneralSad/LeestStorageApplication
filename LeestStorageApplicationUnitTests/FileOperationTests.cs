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
        public void DirectoryLayerDefaultValuesAreCorrect()
        {
            DirectoryLayer directoryLayer = new DirectoryLayer();

            bool result = directoryLayer.CurrentDirectoryLayer.Equals(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\FilesForTransfer") && directoryLayer.Layer == 0;

            Assert.IsTrue(result, "The wrong default values are assigned to the DirectoryLayer class");
        }
    }
}
