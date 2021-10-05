using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LeestStorageServer
{
    class Client
    {
        private NetworkStream stream;
        private TcpClient client;

        public Client(TcpClient client)
        {
            this.client = client;
            this.stream = client.GetStream();
        }

        private static byte[] WrapMessage(byte[] message)
        {
            // Get the length prefix for the message
            byte[] lengthPrefix = BitConverter.GetBytes(message.Length);
            // Concatenate the length prefix and the message
            byte[] ret = new byte[lengthPrefix.Length + message.Length];
            lengthPrefix.CopyTo(ret, 0);
            message.CopyTo(ret, lengthPrefix.Length);
            return ret;
        }

        public void Write(Object message)
        {
            try
            {

                stream.Write(WrapMessage(Encoding.ASCII.GetBytes(ToJsonMessage(message))));
                stream.Flush();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static string ToJsonMessage<T>( T t)
        {
            return JsonConvert.SerializeObject(t); 
        }

        public async Task<string> Read()
        {
            byte[] length = new byte[4];
            this.stream.Read(length, 0, 4);

            int size = BitConverter.ToInt32(length);

            byte[] received = new byte[size];

            int bytesRead = 0;
            while (bytesRead < size)
            {
                int read = await this.stream.ReadAsync(received, bytesRead, received.Length - bytesRead);
                bytesRead += read;
                //Console.WriteLine("ReadMessage: " + read);
            }

            return Encoding.ASCII.GetString(received);
        }

        public void Terminate()
        {
            this.stream.Close();
            this.stream.Dispose();
            this.client.Close();
            this.client.Dispose();
        }
    }
}
