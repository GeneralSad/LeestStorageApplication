using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeestStorageServer
{
    class ClientHandler
    {
        private TcpClient tcpClient;
        private Client client;
        private bool running { get; set; }

        public ClientHandler(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            this.client = new Client(tcpClient);
            
        }

        private async void Run()
        {
            this.running = true;
            while (running)
            {
                try
                {
                    string message = await client.Read();
                    Console.WriteLine(message);
                }
            }
        }
    }
}
