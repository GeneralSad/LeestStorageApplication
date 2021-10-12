﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunicationObjects;
using LeestStorageServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Syroot.Windows.IO;

namespace LeestStorageApplication
{
    class CommunicationHandler
    {
        private TcpClient tcpClient;
        private Client client;
        private bool running { get; set; }
        private ItemListCallback listener;


        public CommunicationHandler(ItemListCallback listener)
        {
            this.tcpClient = new TcpClient();
            this.listener = listener;
            new Thread(Run).Start();
        }

        public async Task SendMessage(object message)
        {
           await this.client.Write(message);
        }

        public async Task Reload()
        {
            await this.client.Write(new { type = "DirectoryRequest" });
            Debug.WriteLine("Reloaded");
        }

        public async Task DownloadRequest(string fileName)
        {
            await this.client.Write(new { type = "FileRequest", fileName});
            Debug.WriteLine("Download: " + fileName);
        }

        private async void Run()
        {

            await tcpClient.ConnectAsync(ServerAddress.IpAddress, ServerAddress.port);

            this.client = new Client(tcpClient);

            await this.Reload();
            this.running = true;
            while (running)
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
            switch (jMessage.Value<string>("type"))
            {
                case "Directory":
                    ObservableCollection<IDirectoryItem> itemList = new ObservableCollection<IDirectoryItem>();
                    foreach (JObject jobject in jMessage.Value<JArray>("files"))
                    {
                        DirectoryFile file =  new DirectoryFile(jobject.Value<string>("Name"), jobject.Value<string>("DetailInfo"), jobject.Value<DateTime>("LastChanged"));

                        string filename = file.Name.Substring(file.Name.LastIndexOf(@"\") + 1);
                        Debug.WriteLine(filename);

                        if (filename.Contains("."))
                        {
                            itemList.Add(new DirectoryFile(filename));
                        } else
                        {
                            itemList.Add(new DirectoryFolder(filename));
                        }

                    }
                    listener.notify(itemList);
                    Debug.WriteLine("received Directory");

                    break;
                case "DirectoryFile":
                    string downloadLocation = new KnownFolder(KnownFolderType.Downloads).Path + @"\" +
                                              jMessage.Value<String>("fileName");
                    Debug.WriteLine("Downloading to: " + downloadLocation);
                    await FileOperation.FileFromByteArray(downloadLocation, await this.client.Read());
                    Console.WriteLine("Received file");
                    break;

            }
        }

        public void Disable()
        {
            this.running = false;
        }

    }
}
