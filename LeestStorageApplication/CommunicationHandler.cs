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

        public void SendMessage(string message)
        {
            this.client.Write(message);
        }

        private async void Run()
        {

            await tcpClient.ConnectAsync(ServerAddress.IpAddress, ServerAddress.port);

            this.client = new Client(tcpClient);

            this.running = true;
            while (running)
            {
                try
                {
                    string message = await client.Read();
                    JObject jMessage = (JObject)JsonConvert.DeserializeObject(message);
                    
                    handleMessage(jMessage);
                    Console.WriteLine(message);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            client.Terminate();
        }

        private void handleMessage(JObject jMessage)
        {
            switch (jMessage.Value<string>("type"))
            {
                case "Directory":
                    Debug.WriteLine("received Directory");
                    break;
                case "File":
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
