using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeestStorageApplication
{
   

    public class Folder : IDirectoryItem
    {

        public string Name { get; set; }

        public Folder(string name)
        {
            this.Name = name;
        }

        public string DetailInfo
        {
            get
            {
                return "Type: Folder";
            }
        }
    }
}
