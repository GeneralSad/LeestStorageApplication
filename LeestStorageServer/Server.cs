using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeestStorageServer
{
    class Server
    {
        private TcpListener listener;
        private List<ClientHandler> clients;
        private int port;
        private bool running { get; set; }


        public Server(int port)
        {
            this.port = port;
            this.clients = new List<ClientHandler>();
            this.listener = new TcpListener(System.Net.IPAddress.Any, port);
        }

        public async void Start()
        {
            this.listener.Start();
            Console.WriteLine($"Server is listening fort clients on port {this.port}.");
            this.running = true;

            while (this.running)
            {
                Console.WriteLine("Waiting for a new connection...");

                try
                {
                    TcpClient client = await this.listener.AcceptTcpClientAsync().ConfigureAwait(false);
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
