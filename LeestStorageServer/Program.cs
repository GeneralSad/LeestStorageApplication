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

            //byte[] fileToByteArray = await FileOperation.FileToByteArray(@"D:\Programs\Fortnite 3\Nioh\archive\archive_10.lnk");

            //Console.WriteLine(fileToByteArray.Length);
            //await FileOperation.FileFromByteArray(@"C:\File\archive.lnk", fileToByteArray);
        }
    }
}
