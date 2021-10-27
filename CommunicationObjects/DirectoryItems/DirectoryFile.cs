using System;

namespace LeestStorageApplication
{
    class DirectoryFile : IDirectoryItem
    {
        public string Name { get; set; }
        public DateTime LastChanged { get; set; }
        private string detailInfo { get; set; }
        public string LastChangedText { get; set; }

        public DirectoryFile(string name)
        {
            Name = name;
        }

        public DirectoryFile(string name, string detailInfo, DateTime lastChanged)
        {
            Name = name;
            DetailInfo = detailInfo;
            LastChanged = lastChanged;
            LastChangedText = "Last Changed: " + lastChanged.ToString();
        }

        public string DetailInfo
        {
            get
            {
                if (!string.IsNullOrEmpty(detailInfo) && !detailInfo.Contains("Type: File"))
                {
                    return "Type: File\n" + detailInfo;
                }
                else
                {
                    return detailInfo.Contains("Type: File") ? detailInfo : "Type: File";
                }

            }
            set => detailInfo = value;
        }

    }
}
