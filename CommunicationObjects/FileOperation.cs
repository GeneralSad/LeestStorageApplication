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

        public static string GetProjectDirectory()
        {
            return Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        }
        // @"\FilesForTransfer"
        public static string[] ReturnFilesFromDirectory(string directory)
        {
            return Directory.GetFileSystemEntries(directory);
        }
    }
}
