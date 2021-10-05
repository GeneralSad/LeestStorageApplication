using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeestStorageApplication
{
   

    public class Folder : IDirectoryItem
    {

        public string name { get; set; }
        public Folder parent { get; set; }
        public List<IDirectoryItem> children { get; set; }

        public Folder(string name, Folder parent)
        {
            this.name = name;
            this.parent = parent;
        }

        public void addChild(IDirectoryItem item)
        {
            children.Add(item);
        }

        public string getFilePath()
        {
            if(parent != null)
            {
                return parent.getFilePath() + "/" + name;
            } else
            {
                return "/" + name;
            }
        }

    }
}
