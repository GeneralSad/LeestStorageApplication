using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;

namespace LeestStorageApplication
{
    class Client
    {

        public Client(int port)
        {
            StartClient(port);
            Console.ReadLine();
        }

        private static async void StartClient(int port)
        {
            TcpClient client = new TcpClient();
            await client.ConnectAsync(IPAddress.Loopback, port);

            LogMessage("Connected to Server");
            using (var networkStream = client.GetStream())
            {

                using (var writer = new StreamWriter(networkStream))
                {

                    using (var reader = new StreamReader(networkStream))
                    {

                        writer.AutoFlush = true;

                        for (int i = 0; i < 10; i++)
                        {
                            await writer.WriteLineAsync(DateTime.Now.ToLongDateString());
                            var dataFromServer = await reader.ReadLineAsync();
                            if (!string.IsNullOrEmpty(dataFromServer))
                            {
                                LogMessage(dataFromServer);
                            }

                        }

                    }

                }

            }

            client?.Close();

        }

        private static void LogMessage(string message, [CallerMemberName] string callername = "")
        {
            Console.WriteLine("{0} - Thread-{1}- {2}", callername, Thread.CurrentThread.ManagedThreadId, message);
        }

    }

}
