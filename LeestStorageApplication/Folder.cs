﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeestStorageApplication
{
   

    public class Folder : IDirectoryItem
    {

        public string Name { get; set; }
        public Folder Parent { get; set; }

        public string path
        {
            get
            {
                return GetFilePath();
            } 
            set
            {
                //TODO: Code for moving file
            }
        }

        public Folder(string name, Folder parent)
        {
            this.Name = name;
            this.Parent = parent;
        }

        public string GetFilePath()
        {
            if(Parent != null)
            {
                return Parent.GetFilePath() + "/" + Name;
            } else
            {
                return "/" + Name;
            }
        }

    }
}
