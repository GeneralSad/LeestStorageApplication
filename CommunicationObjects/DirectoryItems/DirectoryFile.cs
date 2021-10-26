using System;

namespace LeestStorageApplication
{
    class DirectoryFile : IDirectoryItem
    {
        public string Name { get; set; }
        public DateTime LastChanged { get; set; }
        public string DetailInfo { get; set; }
        public string LastChangedText { get; set; }

        public DirectoryFile(string name)
        {
            this.Name = name;
        }

        public DirectoryFile(string name, string detailInfo, DateTime lastChanged)
        {
            this.Name = name;
            this.DetailInfo = detailInfo;
            this.LastChanged = lastChanged;
            this.LastChangedText = "Last changed: " + lastChanged.ToString();
        }

    }
}
