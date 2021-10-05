using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommunicationObjects;

namespace LeestStorageServer
{
    class Server
    {
        private TcpListener listener;
        private List<ClientHandler> clients;

        private bool running { get; set; }


        public Server()
        {
            this.clients = new List<ClientHandler>();
            this.listener = new TcpListener(ServerAddress.IpAddress, ServerAddress.port);
        }

        public async Task Start()
        {
            this.listener.Start();
            Console.WriteLine($"Server is listening fort clients on port {ServerAddress.port}.");
            this.running = true;

            while (this.running)
            {
                Console.WriteLine("Waiting for a new connection...");

                try
                {
                    TcpClient client = await this.listener.AcceptTcpClientAsync().ConfigureAwait(false);
                    Console.WriteLine($"A new client connected: {client}");
                    this.clients.Add(new ClientHandler(client));
                } catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }

            }

            this.listener.Stop();
        }

        public void Disable()
        {
            foreach (ClientHandler client in clients)
            {
                client.Disable();
            }
            this.running = false;
        }
    }
}
