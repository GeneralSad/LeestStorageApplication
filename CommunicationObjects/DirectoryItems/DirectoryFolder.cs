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
            Name = name;
            LastChanged = lastChanged;
            LastChangedText = "Last Changed: " + lastChanged.ToString();
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
