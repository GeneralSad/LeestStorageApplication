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

        public static List<string> ProtectedFiles = new();

        private bool Running { get; set; }


        public Server()
        {
            clients = new List<ClientHandler>();
            listener = new TcpListener(ServerAddress.IpAddress, ServerAddress.port);
        }

        public async Task Start()
        {
            listener.Start();
            Console.WriteLine($"Server is listening fort clients on port {ServerAddress.port}.");
            Running = true;

            while (Running)
            {
                Console.WriteLine("Waiting for a new connection...");

                try
                {
                    TcpClient client = await listener.AcceptTcpClientAsync().ConfigureAwait(false);
                    Console.WriteLine($"A new client connected: {client}");
                    clients.Add(new ClientHandler(client, this));

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }

            }

            listener.Stop();
        }

        public void Disable()
        {
            foreach (ClientHandler client in clients.ToArray())
            {
                client.Disable();
            }
            Running = false;
        }

        public void RemoveClientHandlerFromList(ClientHandler handler)
        {
            clients.Remove(handler);
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
