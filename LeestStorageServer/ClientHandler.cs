using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
                case "DirectoryRequest":
                    Console.WriteLine("Send Updated Directory");
                    String[] files = FileOperation.ReturnFilesFromDirectory(this.directoryLayer.CurrentDirectoryLayer);

                    DirectoryFile[] directoryFiles = FileOperation.FileStringArrayToFileObjectArray(files);
                    var o = new { type = "Directory", files = directoryFiles };

                    await this.client.Write(o);
                    break;
                case "FileRequest":
                    
                    string fileName = jMessage.Value<String>("fileName");
                    Console.WriteLine($"Start sending File {fileName}");

                    byte[] fileToByteArray = await FileOperation.FileToByteArray(this.directoryLayer.CurrentDirectoryLayer + @"\" + fileName);
                    await this.client.Write(new {type = "DirectoryFile", fileName});
                    await this.client.Write(fileToByteArray);
                    break;
                case "FileUploadRequest":
                    Console.WriteLine("Start receiving File");
                    string file = this.directoryLayer.CurrentDirectoryLayer + @"\" + jMessage.Value<String>("fileName");
                    await FileOperation.FileFromByteArray( FileOperation.ReturnAvailableFilePath(file), await this.client.Read());
                    break;
                case "DeleteRequest":
                    string deleteFileLocation = this.directoryLayer.CurrentDirectoryLayer + @"\" + jMessage.Value<String>("fileName");
                    Console.WriteLine($"deleting file {deleteFileLocation}");
                    File.Delete(deleteFileLocation);

                    break;
            }
        }

        public void Disable()
        {
            this.running = false;
        }
    }
}
