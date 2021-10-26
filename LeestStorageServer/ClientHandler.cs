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
        private bool running { get; set; }
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
                await directoryRequest(files);
            }
        }



        private async void Run()
        {
            this.running = true;
            while (running)
            {
                try
                {
                    string message = Encoding.ASCII.GetString(await this.client.Read());
                    JObject jMessage = (JObject)JsonConvert.DeserializeObject(message);

                    await handleMessage(jMessage);

                    Console.WriteLine(message);
                } catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            this.client.Terminate();
        }


        private async Task handleMessage(JObject jMessage)
        {
            if (jMessage == null)
            {
                return;
            }
            switch (jMessage.Value<string>("type"))
            {
                case "IntoDirectoryRequest":
                    await intoDirectoryRequest(jMessage);
                    break;
                case "OutOfDirectoryRequest":
                    await outOfDirectoryRequest();
                    break;
                case "DirectoryRequest":
                    await directoryRequest();
                    break;
                case "FileRequest":
                    await fileRequest(jMessage);
                    break;
                case "FileUploadRequest":
                    await fileUploadRequest(jMessage);
                    break;
                case "DeleteRequest":
                    await deleteRequest(jMessage);
                    break;
                case "CloseConnection":
                    closeConnection();
                    break;
            }
        }


        private async Task intoDirectoryRequest(JObject jMessage)
        {
            Console.WriteLine("updating current directory");
            string directoryName = jMessage.Value<String>("directoryName");
            this.directoryLayer.AddDirectoryLayer(@"\" + directoryName);
            await directoryRequest();
        }

        private async Task outOfDirectoryRequest()
        {
            Console.WriteLine("Going back to former directory");
            this.directoryLayer.RemoveDirectoryLayer();
            await directoryRequest();
        }

        private async Task directoryRequest()
        {
            Console.WriteLine("Sending Updated Directory");
            String[] files = FileOperation.ReturnFilesFromDirectory(this.directoryLayer.CurrentDirectoryLayer);

            DirectoryFile[] directoryFiles = FileOperation.FileStringArrayToFileObjectArray(files);
            var o = new { type = "Directory", files = directoryFiles };

            await this.client.Write(o);
        }

        private async Task directoryRequest(DirectoryFile[] files)
        {
            Console.WriteLine("Sending Updated Directory");
            var o = new { type = "Directory", files};

            await this.client.Write(o);
        }

        private async Task fileRequest(JObject jMessage)
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


        private async Task fileUploadRequest(JObject jMessage)
        {
            Console.WriteLine("Start receiving File");
            string file = this.directoryLayer.CurrentDirectoryLayer + @"\" + jMessage.Value<String>("fileName");
            await FileOperation.FileFromByteArray(FileOperation.ReturnAvailableFilePath(file), await this.client.Read());
            this.callback.RefreshDirectoryForAllClientsInDirectory(this.directoryLayer.CurrentDirectoryLayer);
        }

        private async Task deleteRequest(JObject jMessage)
        {
            string deleteFileLocation = this.directoryLayer.CurrentDirectoryLayer + @"\" + jMessage.Value<String>("fileName");
            if (this.callback.CheckIfFileIsClearToEdit(deleteFileLocation))
            {
                Console.WriteLine($"deleting file {deleteFileLocation}");
                FileOperation.DeleteFileOrDirectory(deleteFileLocation);
                this.callback.RefreshDirectoryForAllClientsInDirectory(this.directoryLayer.CurrentDirectoryLayer);
            }
        }


        private void closeConnection()
        {
            Console.WriteLine($"Closing connection: {this}");
            this.running = false;
            this.callback.RemoveClientHandlerFromList(this);
        }

        public async void Disable()
        {
            await this.client.Write(new { type = "CloseConnection"});
            this.running = false;
            this.callback.RemoveClientHandlerFromList(this);
        }
    }
}
