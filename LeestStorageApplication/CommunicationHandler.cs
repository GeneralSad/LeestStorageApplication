using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunicationObjects;
using LeestStorageServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LeestStorageApplication
{
    class CommunicationHandler
    {
        private TcpClient tcpClient;
        private Client client;
        private bool Running { get; set; }
        private ItemListCallback listener;

        private string DownloadLocation;

        public CommunicationHandler(ItemListCallback listener)
        {
            tcpClient = new TcpClient();
            this.listener = listener;
            new Thread(Run).Start();
        }

        //Send a message to the server
        public async Task SendMessage(object message)
        {
           await client.Write(message);
        }

        //Send a Request to reload
        public async Task Reload()
        {
            await client.Write(new { type = "DirectoryRequest" });
            Debug.WriteLine("Reloaded");
        }

        //Send a request to delete a file or folder
        public async Task DeleteRequest(string fileName)
        {
            await client.Write(new { type = "DeleteRequest", fileName });
            Debug.WriteLine("Download: " + fileName);
        }

        //Send a request to jump into a directory
        public async Task IntoDirectoryRequest(string directoryName)
        {
            await client.Write(new { type = "IntoDirectoryRequest", directoryName });
            Debug.WriteLine("Going into directory: " + directoryName);
        }

        //Send a request to jump into a directory
        public async Task OutOfDirectoryRequest()
        {
            await client.Write(new { type = "OutOfDirectoryRequest" });
            Debug.WriteLine("Out of directory");

        }

        //Send a request to download a file
        public async Task DownloadRequest(string fileName, string downloadLocation)
        {

            DownloadLocation = downloadLocation;
            await client.Write(new { type = "FileRequest", fileName });
            Debug.WriteLine("Download: " + fileName);

        }

        //Send a file to the server
        public async Task UploadRequest(string file)
        {
            byte[] fileToByteArray = await FileOperation.FileToByteArray(file);
            await client.Write(new { type = "FileUploadRequest", fileName = file.Substring(file.LastIndexOf(@"\")) });
            await client.Write(fileToByteArray);
            Debug.WriteLine("Upload: " + file);
        }

        //Send a request to create a folder
        public async Task CreateFolderRequest(string folderName)
        {
            await this.client.Write(new { type = "CreateFolderRequest", folderName = folderName });
            Debug.WriteLine("Created folder: " + folderName);
        }

        //Close the connection to the server
        public async Task CloseConnection()
        {
            await client.Write(new { type = "CloseConnection"});
            Debug.WriteLine("Closing connection");
            Disable();
        }

        private async void Run()
        {

            await tcpClient.ConnectAsync(ServerAddress.IpAddress, ServerAddress.port);

            client = new Client(tcpClient);

            await Reload();
            Running = true;
            while (Running)
            {
                try
                {
                    string message = Encoding.ASCII.GetString(await client.Read());
                    JObject jMessage = (JObject)JsonConvert.DeserializeObject(message);
                    
                    await handleMessage(jMessage);
                    Console.WriteLine("Received: " + message);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            client.Terminate();
        }

        private async Task handleMessage(JObject jMessage)
        {
            if (jMessage == null)
            {
                return;
            }
            switch (jMessage.Value<string>("type"))
            {
                case "Directory":
                    Directory(jMessage);
                    break;
                case "DirectoryFile":
                    await DirectoryFile(jMessage);
                    break;
                case "CloseConnection":
                    Disable();
                    break;

            }
        }

        private void Directory(JObject jMessage)
        {
            ObservableCollection<IDirectoryItem> itemList = new();
            foreach (JObject jobject in jMessage.Value<JArray>("files"))
            {

                DirectoryFile file = new(jobject.Value<string>("Name"), jobject.Value<string>("DetailInfo"), jobject.Value<DateTime>("LastChanged"));
                string filename = file.Name.Substring(file.Name.LastIndexOf(@"\") + 1);
                Debug.WriteLine(filename);

                if (filename.Contains("."))
                {
                    itemList.Add(new DirectoryFile(jobject.Value<string>("Name"), jobject.Value<string>("DetailInfo"), jobject.Value<DateTime>("LastChanged")));
                }
                else
                {
                    itemList.Add(new DirectoryFolder(jobject.Value<string>("Name"), jobject.Value<DateTime>("LastChanged")));
                }

            }
            listener.Notify(itemList);
            Debug.WriteLine("received Directory");
        }

        private async Task DirectoryFile(JObject jMessage)
        {
            string downloadLocation = DownloadLocation;
            Debug.WriteLine("Downloading to: " + downloadLocation);
            await FileOperation.FileFromByteArray(FileOperation.ReturnAvailableFilePath(downloadLocation + @"\" + jMessage.Value<String>("fileName")), await client.Read());
            Console.WriteLine("Received file");
        }

        public void Disable()
        {
            Running = false;
        }

    }
}
