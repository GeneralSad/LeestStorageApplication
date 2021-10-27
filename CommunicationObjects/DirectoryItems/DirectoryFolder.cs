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
        public DateTime LastChanged { get; set; }
        public string LastChangedText { get; set; } = "";

        public DirectoryFolder(string name)
        {
            this.Name = name;
        }

        public DirectoryFolder(string name, DateTime lastChanged)
        {
            this.Name = name;
            this.LastChanged = lastChanged;
            this.LastChangedText = "Last Changed: " + lastChanged.ToString();
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
