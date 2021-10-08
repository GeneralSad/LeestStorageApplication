using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunicationObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LeestStorageServer
{
    class ClientHandler : MessageCallback
    {
        private TcpClient tcpClient;
        private Client client;
        private bool running { get; set; }


        public ClientHandler(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            this.client = new Client(tcpClient);

            new Thread(Run).Start();
        }

        public async Task sendMessage(string message)
        {
            await this.client.Write(message);
        }

        private async void Run()
        {
            String[] files = FileOperation.ReturnFilesFromDirectory(FileOperation.GetProjectDirectory() + @"\FilesForTransfer");
            var o = new {type = "Directory", files = files};
            await this.client.Write(o);
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
                    break;
                case "FileRequest":
                    Console.WriteLine("Start sending File");
                    await this.client.Write(new {type = "File"});
                    byte[] fileToByteArray = await FileOperation.FileToByteArray(@"E:\download\GitKrakenSetup.exe");
                    this.client.Write(fileToByteArray);
                    break;
                case "FileUploadRequest":
                    Console.WriteLine("Start receiving File");
                    //The code below is placeholder, might even break something :)
                    await FileOperation.FileFromByteArray( Environment.CurrentDirectory + @"//file", await this.client.Read());
                    break;

            }
        }

        public void Disable()
        {
            this.running = false;
        }

        public void MessageReceived(string message)
        {
            
        }
    }
}
