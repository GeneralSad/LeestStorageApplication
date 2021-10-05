using System;
using System.Threading.Tasks;
using CommunicationObjects;

namespace LeestStorageServer
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            //Server server = new Server();
            //await server.Start();

            byte[] fileToByteArray = await FileOperation.FileToByteArray(@"C:\Users\ikben\Downloads\EpicInstaller-12.1.7.msi");

            Console.WriteLine(fileToByteArray.Length);
        }
    }
}
