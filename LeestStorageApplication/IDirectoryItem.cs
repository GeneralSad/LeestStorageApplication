using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeestStorageApplication
{
    interface IDirectoryItem
    {

        public string name { get; set; }
        public Folder parent { get; set; }
        public string getFilePath();

    }
}
