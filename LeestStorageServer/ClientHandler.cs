using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Threading;
using CommunicationObjects;

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

        public void sendMessage(string message)
        {
            this.client.Write(message);
        }

        private async void Run()
        {
            this.running = true;
            while (running)
            {
                try
                {
                    string message = JsonConverter.DeserializeObject(await client.Read());

                    Console.WriteLine(message);
                } catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            client.Terminate();
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
