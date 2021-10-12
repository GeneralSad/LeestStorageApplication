using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeestStorageApplication
{
    public interface IDirectoryItem
    {

        public string Name { get; set; }
        public string DetailInfo { get; }

    }
}
