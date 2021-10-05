using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationObjects
{
    class FileOperation
    {
        public static async Task FileFromByteArray(string directory, byte[] file)
        {
            await File.WriteAllBytesAsync(directory, file);
        }

        public static async Task<byte[]> FileToByteArray(string FilePath)
        {

            FileStream stream = File.OpenRead(FilePath);
            byte[] fileByteArray = new byte[stream.Length];

            long size = stream.Length;
            int bytesRead = 0;

            while (bytesRead < size)
            {
                int read = await stream.ReadAsync(fileByteArray, bytesRead, fileByteArray.Length - bytesRead);
                bytesRead += read;
            }

            return fileByteArray;
        }
    }
}
