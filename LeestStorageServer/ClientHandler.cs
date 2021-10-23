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


        public ClientHandler(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            this.client = new Client(tcpClient);
            this.directoryLayer = new DirectoryLayer();
            new Thread(Run).Start();
        }

        public async Task sendMessage(string message)
        {
            await this.client.Write(message);
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

                case "DeleteRequest":
                    await DeleteRequest(jMessage);
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

        private async Task FileRequest(JObject jMessage)
        {
            string fileName = jMessage.Value<String>("fileName");
            Console.WriteLine($"Start sending File {fileName}");

            byte[] fileToByteArray = await FileOperation.FileToByteArray(this.directoryLayer.CurrentDirectoryLayer + @"\" + fileName);
            await this.client.Write(new { type = "DirectoryFile", fileName });
            await this.client.Write(fileToByteArray);
        }


        private async Task FileUploadRequest(JObject jMessage)
        {
            Console.WriteLine("Start receiving File");
            string file = this.directoryLayer.CurrentDirectoryLayer + @"\" + jMessage.Value<String>("fileName");
            await FileOperation.FileFromByteArray(FileOperation.ReturnAvailableFilePath(file), await this.client.Read());
        }

        private async Task DeleteRequest(JObject jMessage)
        {
            string deleteFileLocation = this.directoryLayer.CurrentDirectoryLayer + @"\" + jMessage.Value<String>("fileName");
            Console.WriteLine($"deleting file {deleteFileLocation}");
            File.Delete(deleteFileLocation);
            await this.DirectoryRequest();
        }


        public void Disable()
        {
            this.running = false;
        }
    }
}
