using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeestStorageApplication
{
    class File : IDirectoryItem
    {
        public string Name { get; set; }

        public File(string name)
        {
            this.Name = name;
        }

        public string DetailInfo
        {
            get
            {
                return "Type: File";
            }
        }
    }
}
