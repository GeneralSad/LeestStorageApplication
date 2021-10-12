using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeestStorageApplication
{
    class DirectoryFile : IDirectoryItem
    {
        public string Name { get; set; }
        public string LastChanged { get; set; }

        public DirectoryFile(string name)
        {
            this.Name = name;
        }

        public string DetailInfo
        {
            get
            {
                return "Type: DirectoryFile";
            }
        }
    }
}
