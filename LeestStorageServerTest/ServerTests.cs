using LeestStorageServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeestStorageApplicationUnitTests
{
    [TestClass]
    public class ServerTests
    {


        [TestMethod]
        public void AddProtectedFileTest()
        {
            Server server = new Server();
            string file = "testAddProtectedFile";
            
            server.AddFileBeingEdited(file);

            Assert.IsFalse(server.CheckIfFileIsClearToEdit(file));
        }

        [TestMethod]
        public void RemoveProtectedFileTest()
        {
            Server server = new Server();
            string file = "testRemoveProtectedFile";

            server.AddFileBeingEdited(file);
            server.RemoveFileBeingEdited(file);

            Assert.IsTrue(server.CheckIfFileIsClearToEdit(file));
        }


    }
}
