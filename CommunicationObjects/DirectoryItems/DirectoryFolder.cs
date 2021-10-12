using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeestStorageApplication
{
   

    public class DirectoryFolder : IDirectoryItem
    {

        public string Name { get; set; }

        public DirectoryFolder(string name)
        {
            this.Name = name;
        }

        public string DetailInfo
        {
            get
            {
                return "Type: DirectoryFolder";
            }
        }
    }
}
