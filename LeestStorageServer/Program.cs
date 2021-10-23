using System;
using System.Threading.Tasks;
using CommunicationObjects;

namespace LeestStorageServer
{
    class Program
    {

        private static Server server;
        public static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            server = new Server();
            await server.Start();
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            server.Disable();
        }
    }
}
