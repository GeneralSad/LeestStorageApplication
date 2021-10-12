using System;
using System.Threading.Tasks;
using CommunicationObjects;

namespace LeestStorageServer
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Server server = new Server();
            await server.Start();

           

            //byte[] fileToByteArray = await FileOperation.FileToByteArray(@"E:\download\GitKrakenSetup.exe");

            //Console.WriteLine(fileToByteArray.Length);
            //await FileOperation.FileFromByteArray(@"C:\File\KrakenSetup.exe", fileToByteArray);
        }
    }
}
