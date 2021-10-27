using System;

namespace LeestStorageApplication
{

    public class DirectoryFolder : IDirectoryItem
    {

        public string Name { get; set; }
        public DateTime LastChanged { get; set; }
        public string LastChangedText { get; set; } = "";

        public DirectoryFolder(string name)
        {
            Name = name;
        }

        public DirectoryFolder(string name, DateTime lastChanged)
        {
            this.Name = name;
            this.LastChanged = lastChanged;
            this.LastChangedText = "Last changed: " + lastChanged;
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
