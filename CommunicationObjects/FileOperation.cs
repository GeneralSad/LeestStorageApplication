﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LeestStorageApplication;

namespace CommunicationObjects
{
    class FileOperation
    {
        public string CurrentDirectoryLayer { get; set; }
        public int Layer { get; private set; }


        public FileOperation()
        {
            this.CurrentDirectoryLayer = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\FilesForTransfer";
            this.Layer = 0;
        }

        public void AddDirectoryLayer(string directory)
        {
            this.CurrentDirectoryLayer += directory;
            this.Layer++;
        }

        public void RemoveDirectoryLayer()
        {
            if (this.Layer > 0)
            {
                this.CurrentDirectoryLayer =
                    this.CurrentDirectoryLayer.Substring(0, this.CurrentDirectoryLayer.LastIndexOf("/"));
                this.Layer--;
            }
        }

        public static async Task FileFromByteArray(string directory, byte[] file)
        {
            await File.WriteAllBytesAsync(directory, file);
        }

        public static async Task<byte[]> FileToByteArray(string FilePath)
        {
            return await File.ReadAllBytesAsync(FilePath);
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
    }
}
