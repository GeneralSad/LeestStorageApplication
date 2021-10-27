using System;

namespace LeestStorageApplication
{

    //Interface for directory items
    public interface IDirectoryItem
    {
        public string Name { get; set; }
        public string DetailInfo { get; }

    }
}
