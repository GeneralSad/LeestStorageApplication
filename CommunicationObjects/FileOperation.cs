using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
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

        public static void deleteFile(string filePath)
        {
            if (filePath.Contains("."))
            {
                File.Delete(filePath);
            }
            else
            {
                foreach (var fileInDirectoryPath in ReturnFilesFromDirectory(filePath))
                {
                    deleteFile(fileInDirectoryPath);
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
                DirectoryFile directoryFile = new DirectoryFile(file.Substring(file.LastIndexOf(@"\") + 1));
                directoryFile.LastChanged = File.GetLastWriteTime(file);
                directoryFiles[i] = directoryFile;
                i++;
            }

            return directoryFiles;
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
