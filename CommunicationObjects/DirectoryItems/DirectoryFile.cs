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
            this.Name = name;
        }

        public DirectoryFile(string name, string detailInfo, DateTime lastChanged)
        {
            this.Name = name;
            this.DetailInfo = detailInfo;
            this.LastChanged = lastChanged;
            this.LastChangedText = "Last Changed: " + lastChanged.ToString();
        }

        public string DetailInfo
        {
            get
            {
                if (!string.IsNullOrEmpty(detailInfo) && !detailInfo.Contains("Type: File"))
                {
                    return "Type: File\n" + detailInfo;
                }
                else if (detailInfo.Contains("Type: File"))
                {
                    return detailInfo;
                }
                else
                {
                    return "Type: File";
                }
                
            }
            set
            {
                this.detailInfo = value;
            }
        }

    }
}
