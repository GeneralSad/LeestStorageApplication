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
            return await File.ReadAllBytesAsync(FilePath);
        }
    }
}
