using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeestStorageApplicationUnitTests
{
    [TestClass]
    public class DirectoryLayerTests
    {

        
        [TestMethod]
        public void DirectoryLayerDefaultValuesAreCorrect()
        {
            DirectoryLayer directoryLayer = new DirectoryLayer();
            bool result = directoryLayer.CurrentDirectoryLayer.Equals(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\FilesForTransfer") && directoryLayer.Layer == 0;

            Assert.IsTrue(result, "The wrong default values are assigned to the DirectoryLayer class");
        }

        [TestMethod]
        public void DirectoryLayerAddLayerMethodWorks()
        {
            DirectoryLayer directoryLayer = new DirectoryLayer();
            directoryLayer.AddDirectoryLayer(@"\ExtraLayer");
            bool result = directoryLayer.CurrentDirectoryLayer.Equals(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\FilesForTransfer" + @"\ExtraLayer") && directoryLayer.Layer == 1;

            Assert.IsTrue(result, "DirectoryLayer add layer method does not function properly");
        }

        [TestMethod]
        public void DirectoryLayerRemoveLayerMethodWorks()
        {
            DirectoryLayer directoryLayer = new DirectoryLayer();
            directoryLayer.AddDirectoryLayer(@"\ExtraLayer");
            directoryLayer.RemoveDirectoryLayer();
            bool result = directoryLayer.CurrentDirectoryLayer.Equals(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\FilesForTransfer") && directoryLayer.Layer == 0;

            Assert.IsTrue(result, "DirectoryLayer remove layer method does not function properly");
        }
    }
}
