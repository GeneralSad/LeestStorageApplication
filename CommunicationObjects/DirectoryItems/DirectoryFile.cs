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
        public string DetailInfo { get; }
        public DateTime LastChanged { get; set; }


        public DirectoryFile(string name)
        {
            this.Name = name;
        }

        public DirectoryFile(string name, string detailInfo, DateTime lastChanged)
        {
            this.Name = name;
            this.DetailInfo = detailInfo;
            this.LastChanged = lastChanged;
        }


    }
}
