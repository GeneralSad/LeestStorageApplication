using System;
using System.IO;
using System.Threading.Tasks;
using LeestStorageApplication;

namespace CommunicationObjects
{
    class FileOperation
    {

        public static async Task FileFromByteArray(string filePath, byte[] file)
        {
            await File.WriteAllBytesAsync(filePath, file);
        }

        public static async Task<byte[]> FileToByteArray(string filePath)
        {
            return await File.ReadAllBytesAsync(filePath);
        }

        public static void DeleteFileOrDirectory(string filePath)
        {
            if (filePath.Contains("."))
            {
                File.Delete(filePath);
            }
            else
            {
                foreach (var fileInDirectoryPath in ReturnFilesFromDirectory(filePath))
                {
                    DeleteFileOrDirectory(fileInDirectoryPath);
                }
                Directory.Delete(filePath);
            }
        }
        
        public static DirectoryFile[] FileStringArrayToFileObjectArray(string[] files)
        {
            DirectoryFile[] directoryFiles = new DirectoryFile[files.Length];
            int i = 0;
            foreach (string file in files)
            {
                DirectoryFile directoryFile = new DirectoryFile(file.Substring(file.LastIndexOf(@"\") + 1), "", File.GetLastWriteTime(file));
                directoryFiles[i] = directoryFile;
                i++;
            }

            return directoryFiles;
        }
        
        public static void CreateDirectory(string DirectoryPath)
        {
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(DirectoryPath))
                {
                    Console.WriteLine("That path exists already.");
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(DirectoryPath);
                Console.WriteLine("The directory was created at {0}.", Directory.GetCreationTime(DirectoryPath));

            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }

        }

        // @"\FilesForTransfer"
        public static string[] ReturnFilesFromDirectory(string directory)
        {
            return Directory.GetFileSystemEntries(directory);
        }

        //This method puts (1) after the name of the file to make sure it does not overwrite an old file. This method only works for files with a '.'
        public static string ReturnAvailableFilePath(string filePath)
        {
            if (File.Exists(filePath))
            {
                if (filePath.Contains("."))
                {
                    int dotLocation = filePath.IndexOf(".");
                    return ReturnAvailableFilePath(filePath.Insert(dotLocation, " (1)"));
                }
                else
                {
                    return filePath;
                }

            }

            return filePath;
        }
    }
}
