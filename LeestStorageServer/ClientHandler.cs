using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

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

            new Thread(Run).Start();
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
                } catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            client.terminate();
        }

        private void disable()
        {
            this.running = false;
        }

    }
}
