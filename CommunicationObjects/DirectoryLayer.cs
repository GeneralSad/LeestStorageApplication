using System;
using System.IO;

namespace CommunicationObjects
{
    class DirectoryLayer
    {
        public string CurrentDirectoryLayer { get; set; }
        public int Layer { get; private set; }


        public DirectoryLayer()
        {
            CurrentDirectoryLayer = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\FilesForTransfer";
            Layer = 0;
        }

        //Add a layer to the directory
        public void AddDirectoryLayer(string directory)
        {
            CurrentDirectoryLayer += directory;
            Layer++;
        }

        //Remove a layer from the directory
        public void RemoveDirectoryLayer()
        {
            if (Layer > 0)
            {
                CurrentDirectoryLayer =
                    CurrentDirectoryLayer.Substring(0, this.CurrentDirectoryLayer.LastIndexOf(@"\"));
                Layer--;
            }
        }
    }
}
