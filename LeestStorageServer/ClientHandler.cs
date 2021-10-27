using System;
using System.Net.Sockets;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunicationObjects;
using LeestStorageApplication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LeestStorageServer
{
    class ClientHandler
    {
        private TcpClient tcpClient;
        private Client client;
        private bool Running { get; set; }
        private DirectoryLayer directoryLayer;
        private ServerCallback callback;


        public ClientHandler(TcpClient tcpClient, ServerCallback callback)
        {
            this.tcpClient = tcpClient;
            client = new Client(tcpClient);
            directoryLayer = new DirectoryLayer();
            this.callback = callback;
            new Thread(Run).Start();
        }

        //Updates the directory if the current directory is equal to the given directory
        public async Task UpdateDirectoryIfEqual(string directory, DirectoryFile[] files)
        {
            if (directoryLayer.CurrentDirectoryLayer == directory)
            {
                await DirectoryRequest(files);
            }
        }

        private async void Run()
        {
            Running = true;
            while (Running)
            {
                try
                {
                    string message = Encoding.ASCII.GetString(await client.Read());
                    JObject jMessage = (JObject)JsonConvert.DeserializeObject(message);

                    await HandleMessage(jMessage);

                    Console.WriteLine(message);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            this.client.Terminate();
        }

        //When a message has been received this method will sort out what to do with this message
        private async Task HandleMessage(JObject jMessage)
        {
            if (jMessage == null)
            {
                return;
            }
            switch (jMessage.Value<string>("type"))
            {
                case "IntoDirectoryRequest":
                    await IntoDirectoryRequest(jMessage);
                    break;
                case "OutOfDirectoryRequest":
                    await OutOfDirectoryRequest();
                    break;
                case "DirectoryRequest":
                    await DirectoryRequest();
                    break;
                case "FileRequest":
                    await FileRequest(jMessage);
                    break;
                case "FileUploadRequest":
                    await FileUploadRequest(jMessage);
                    break;
                case "CreateFolderRequest":
                    CreateFolderRequest(jMessage);
                    break;
                case "DeleteRequest":
                    await DeleteRequest(jMessage);
                    break;
                case "CloseConnection":
                    CloseConnection();
                    break;
            }
        }

        //Jump into the requested directory
        private async Task IntoDirectoryRequest(JObject jMessage)
        {
            Console.WriteLine("updating current directory");
            string directoryName = jMessage.Value<String>("directoryName");
            directoryLayer.AddDirectoryLayer(@"\" + directoryName);
            await DirectoryRequest();
        }

        //Jump out of the current directory
        private async Task OutOfDirectoryRequest()
        {
            Console.WriteLine("Going back to former directory");
            directoryLayer.RemoveDirectoryLayer();
            await DirectoryRequest();
        }

        //Get all files from the directory
        private async Task DirectoryRequest()
        {
            Console.WriteLine("Sending Updated Directory");
            string[] files = FileOperation.ReturnFilesFromDirectory(this.directoryLayer.CurrentDirectoryLayer);

            DirectoryFile[] directoryFiles = FileOperation.FileStringArrayToFileObjectArray(files);
            var o = new { type = "Directory", files = directoryFiles };

            await client.Write(o);
        }

        private async Task DirectoryRequest(DirectoryFile[] files)
        {
            Console.WriteLine("Sending Updated Directory");
            var o = new { type = "Directory", files};

            await client.Write(o);
        }

        //Handle request for a specific file
        private async Task FileRequest(JObject jMessage)
        {
            string fileName = jMessage.Value<string>("fileName");
            string directory = directoryLayer.CurrentDirectoryLayer + @"\" + fileName;
            Console.WriteLine($"Start sending File {fileName}");
            callback.AddFileBeingEdited(directory);
            byte[] fileToByteArray = await FileOperation.FileToByteArray(directory);
            callback.RemoveFileBeingEdited(directory);
            await client.Write(new { type = "DirectoryFile", fileName });
            await client.Write(fileToByteArray);
        }

        //Handle request to receive file
        private async Task FileUploadRequest(JObject jMessage)
        {
            Console.WriteLine("Start receiving File");
            string file = directoryLayer.CurrentDirectoryLayer + @"\" + jMessage.Value<String>("fileName");
            await FileOperation.FileFromByteArray(FileOperation.ReturnAvailableFilePath(file), await client.Read());
            callback.RefreshDirectoryForAllClientsInDirectory(directoryLayer.CurrentDirectoryLayer);
        }

        //Handle request to create a folder
        private void CreateFolderRequest(JObject jMessage)
        {
            Console.WriteLine("Start receiving Folder");
            string FolderPath = directoryLayer.CurrentDirectoryLayer + @"\" + jMessage.Value<string>("folderName");
            FileOperation.CreateDirectory(FolderPath);
            callback.RefreshDirectoryForAllClientsInDirectory(directoryLayer.CurrentDirectoryLayer);
        }

        //Handle a request to delete a file or folder
        private async Task DeleteRequest(JObject jMessage)
        {
            string deleteFileLocation = directoryLayer.CurrentDirectoryLayer + @"\" + jMessage.Value<string>("fileName");
            if (callback.CheckIfFileIsClearToEdit(deleteFileLocation))
            {
                Console.WriteLine($"deleting file {deleteFileLocation}");
                FileOperation.DeleteFileOrDirectory(deleteFileLocation);
                callback.RefreshDirectoryForAllClientsInDirectory(directoryLayer.CurrentDirectoryLayer);
            }
        }

        private void CloseConnection()
        {
            Console.WriteLine($"Closing connection: {this}");
            Running = false;
            callback.RemoveClientHandlerFromList(this);
        }

        public async void Disable()
        {
            await client.Write(new { type = "CloseConnection"});
            Running = false;
            callback.RemoveClientHandlerFromList(this);
        }
    }
}
