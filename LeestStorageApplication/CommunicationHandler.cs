using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunicationObjects;
using LeestStorageServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LeestStorageApplication
{
    class CommunicationHandler : MessageCallback
    {
        private TcpClient tcpClient;
        private Client client;
        private bool running { get; set; }


        public CommunicationHandler()
        {
            this.tcpClient = new TcpClient();

            new Thread(Run).Start();
        }

        public async Task SendMessage(object message)
        {
           await this.client.Write(message);
        }

        private async void Run()
        {

            await tcpClient.ConnectAsync(ServerAddress.IpAddress, ServerAddress.port);

            this.client = new Client(tcpClient);
            await this.SendMessage(new { type = "FileRequest" });

            this.running = true;
            while (running)
            {
                try
                {
                    string message = Encoding.ASCII.GetString(await client.Read());
                    JObject jMessage = (JObject)JsonConvert.DeserializeObject(message);
                    
                    await handleMessage(jMessage);
                    Console.WriteLine(message);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            client.Terminate();
        }

        private async Task handleMessage(JObject jMessage)
        {
            switch (jMessage.Value<string>("type"))
            {
                case "Directory":
                    foreach (string file in jMessage.Value<JArray>("files"))
                    {
                        Debug.WriteLine(file.Substring(file.LastIndexOf(@"\") + 1));
                    }
                    Debug.WriteLine("received Directory");

                    break;
                case "File":
                    await FileOperation.FileFromByteArray(@"C:\File\KrakenSetup.exe", await this.client.Read());
                    Console.WriteLine("Received file");
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
