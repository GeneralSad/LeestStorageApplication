using System;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;
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
            this.client = new Client(tcpClient);
            this.directoryLayer = new DirectoryLayer();
            this.callback = callback;
            new Thread(Run).Start();
        }

        public async Task UpdateDirectoryIfEqual(string directory, DirectoryFile[] files)
        {
            if (this.directoryLayer.CurrentDirectoryLayer == directory)
            {
                await DirectoryRequest(files);
            }
        }

        private async void Run()
        {
            this.Running = true;
            while (Running)
            {
                try
                {
                    string message = Encoding.ASCII.GetString(await this.client.Read());
                    JObject jMessage = (JObject)JsonConvert.DeserializeObject(message);

                    await HandleMessage(jMessage);

                    Console.WriteLine(message);
                } catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            this.client.Terminate();
        }


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

        private async Task IntoDirectoryRequest(JObject jMessage)
        {
            Console.WriteLine("updating current directory");
            string directoryName = jMessage.Value<String>("directoryName");
            this.directoryLayer.AddDirectoryLayer(@"\" + directoryName);
            await DirectoryRequest();
        }

        private async Task OutOfDirectoryRequest()
        {
            Console.WriteLine("Going back to former directory");
            this.directoryLayer.RemoveDirectoryLayer();
            await DirectoryRequest();
        }

        private async Task DirectoryRequest()
        {
            Console.WriteLine("Sending Updated Directory");
            String[] files = FileOperation.ReturnFilesFromDirectory(this.directoryLayer.CurrentDirectoryLayer);

            DirectoryFile[] directoryFiles = FileOperation.FileStringArrayToFileObjectArray(files);
            var o = new { type = "Directory", files = directoryFiles };

            await this.client.Write(o);
        }

        private async Task DirectoryRequest(DirectoryFile[] files)
        {
            Console.WriteLine("Sending Updated Directory");
            var o = new { type = "Directory", files};

            await this.client.Write(o);
        }

        private async Task FileRequest(JObject jMessage)
        {
            string fileName = jMessage.Value<String>("fileName");
            string directory = this.directoryLayer.CurrentDirectoryLayer + @"\" + fileName;
                               Console.WriteLine($"Start sending File {fileName}");
            this.callback.AddFileBeingEdited(directory);
            byte[] fileToByteArray = await FileOperation.FileToByteArray(directory);
            this.callback.RemoveFileBeingEdited(directory);
            await this.client.Write(new { type = "DirectoryFile", fileName });
            await this.client.Write(fileToByteArray);
        }


        private async Task FileUploadRequest(JObject jMessage)
        {
            Console.WriteLine("Start receiving File");
            string file = this.directoryLayer.CurrentDirectoryLayer + @"\" + jMessage.Value<String>("fileName");
            await FileOperation.FileFromByteArray(FileOperation.ReturnAvailableFilePath(file), await this.client.Read());
            this.callback.RefreshDirectoryForAllClientsInDirectory(this.directoryLayer.CurrentDirectoryLayer);
        }

        private void CreateFolderRequest(JObject jMessage)
        {
            Console.WriteLine("Start receiving Folder");
            string FolderPath = this.directoryLayer.CurrentDirectoryLayer + @"\" + jMessage.Value<String>("folderName");
            FileOperation.CreateDirectory(FolderPath);
            this.callback.RefreshDirectoryForAllClientsInDirectory(this.directoryLayer.CurrentDirectoryLayer);
        }

        private async Task DeleteRequest(JObject jMessage)
        {
            string deleteFileLocation = this.directoryLayer.CurrentDirectoryLayer + @"\" + jMessage.Value<String>("fileName");
            if (this.callback.CheckIfFileIsClearToEdit(deleteFileLocation))
            {
                Console.WriteLine($"deleting file {deleteFileLocation}");
                FileOperation.DeleteFileOrDirectory(deleteFileLocation);
                this.callback.RefreshDirectoryForAllClientsInDirectory(this.directoryLayer.CurrentDirectoryLayer);
            }
        }


        private void CloseConnection()
        {
            Console.WriteLine($"Closing connection: {this}");
            this.Running = false;
            this.callback.RemoveClientHandlerFromList(this);
        }

        public async void Disable()
        {
            await this.client.Write(new { type = "CloseConnection"});
            this.Running = false;
            this.callback.RemoveClientHandlerFromList(this);
        }
    }
}
