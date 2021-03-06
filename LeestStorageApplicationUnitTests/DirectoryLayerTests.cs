using System;
using System.IO;
using CommunicationObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeestStorageApplicationUnitTests
{
    [TestClass]
    public class DirectoryLayerTests
    {
        [TestMethod]
        public void DirectoryLayerDefaultValuesAreCorrectTest()
        {
            DirectoryLayer directoryLayer = new DirectoryLayer();

            bool result = directoryLayer.CurrentDirectoryLayer.Equals(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\FilesForTransfer") && directoryLayer.Layer == 0;

            Assert.IsTrue(result, "The wrong default values are assigned to the DirectoryLayer class");
        }

        [TestMethod]
        public void DirectoryLayerAddLayerMethodTest()
        {
            DirectoryLayer directoryLayer = new DirectoryLayer();
            directoryLayer.AddDirectoryLayer(@"\ExtraLayer");

            bool result = directoryLayer.CurrentDirectoryLayer.Equals(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\FilesForTransfer" + @"\ExtraLayer") && directoryLayer.Layer == 1;

            Assert.IsTrue(result, "DirectoryLayer add layer method does not function properly");
        }

        [TestMethod]
        public void DirectoryLayerRemoveLayerMethodTest()
        {
            DirectoryLayer directoryLayer = new DirectoryLayer();
            directoryLayer.AddDirectoryLayer(@"\ExtraLayer");
            directoryLayer.RemoveDirectoryLayer();

            bool result = directoryLayer.CurrentDirectoryLayer.Equals(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\FilesForTransfer") && directoryLayer.Layer == 0;

            Assert.IsTrue(result, "DirectoryLayer remove layer method does not function properly");
        }
    }
}
