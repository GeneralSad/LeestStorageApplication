using System;
using System.Threading.Tasks;

namespace LeestStorageServer
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Server server = new Server();
            await server.Start();
        }
    }
}
