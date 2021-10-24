using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommunicationObjects;
using LeestStorageApplication;

namespace LeestStorageServer
{
    class Server : ServerCallback
    {
        private TcpListener listener;
        private List<ClientHandler> clients;

        public static List<string> ProtectedFiles = new List<string>();

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
                    this.clients.Add(new ClientHandler(client, this));

                } catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }

            }

            this.listener.Stop();
        }

        public void Disable()
        {
            foreach (ClientHandler client in clients.ToArray())
            {
                client.Disable();
            }
            this.running = false;
        }

        public void RemoveClientHandlerFromList(ClientHandler handler)
        {
            this.clients.Remove(handler);
        }

        public async void RefreshDirectoryForAllClientsInDirectory(string directory)
        {

            string[] filesFromDirectory = FileOperation.ReturnFilesFromDirectory(directory);
            DirectoryFile[] files = FileOperation.FileStringArrayToFileObjectArray(filesFromDirectory);
            foreach (ClientHandler client in clients)
            {
                await client.UpdateDirectoryIfEqual(directory, files);
            }
        }

        public bool CheckIfFileIsClearToEdit(string file)
        {
            return !ProtectedFiles.Contains(file);
        }

        public void AddFileBeingEdited(string file)
        {
            ProtectedFiles.Add(file);
        }

        public void RemoveFileBeingEdited(string file)
        {
            ProtectedFiles.Remove(file);
        }
    }
}
