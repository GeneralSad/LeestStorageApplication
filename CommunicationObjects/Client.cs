using System;
using System.Net.Sockets;
using System.Diagnostics;
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
            stream = client.GetStream();
        }

        //Wrap the message to make it ready to send
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

        //Write the object
        public async Task Write(object message)
        {
            try
            {
                await stream.WriteAsync(WrapMessage(Encoding.ASCII.GetBytes(ToJsonMessage(message))));
                await stream.FlushAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        //Write the message
        public async Task Write(byte[] message)
        {
            try
            {
                await stream.WriteAsync(WrapMessage(message));
                await stream.FlushAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        //Convert the object to json
        public static string ToJsonMessage<T>( T t)
        {
            return JsonConvert.SerializeObject(t);
        }

        //Read the stream for messages
        public async Task<byte[]> Read()
        {
            byte[] length = new byte[4];
            this.stream.Read(length, 0, 4);

            int size = BitConverter.ToInt32(length);

            byte[] received = new byte[size];

            int bytesRead = 0;
            while (bytesRead < size)
            {
                int read = await stream.ReadAsync(received, bytesRead, received.Length - bytesRead);
                bytesRead += read;
                //Console.WriteLine("ReadMessage: " + read);
            }

            return received;
        }

        //Terminate connection to the client
        public void Terminate()
        {
            client.Close();
        }
    }
}
