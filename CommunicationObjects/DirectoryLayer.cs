using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommunicationObjects
{
    class DirectoryLayer
    {
        public string CurrentDirectoryLayer { get; set; }
        public int Layer { get; private set; }


        public DirectoryLayer()
        {
            this.CurrentDirectoryLayer = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\FilesForTransfer";
            this.Layer = 0;
        }

        //Add a layer to the directory
        public void AddDirectoryLayer(string directory)
        {
            this.CurrentDirectoryLayer += directory;
            this.Layer++;
        }

        //Remove a layer from the directory
        public void RemoveDirectoryLayer()
        {
            if (this.Layer > 0)
            {
                this.CurrentDirectoryLayer =
                    this.CurrentDirectoryLayer.Substring(0, this.CurrentDirectoryLayer.LastIndexOf(@"\"));
                this.Layer--;
            }
        }
    }
}
