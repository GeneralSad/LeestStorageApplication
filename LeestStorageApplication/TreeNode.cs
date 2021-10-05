using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeestStorageApplication
{
    class TreeNode<IDirectoryItem>
    {
        private readonly IDirectoryItem item;
        private readonly List<TreeNode<IDirectoryItem>> children = new List<TreeNode<IDirectoryItem>>();

        public TreeNode(IDirectoryItem item)
        {
            this.item = item;
        }

        public TreeNode<IDirectoryItem> this[int i]
        {
            get { return children[i]; }
        }

        public TreeNode<IDirectoryItem> Parent { get; private set; }

        public IDirectoryItem Value { get { return item; } }

        public void AddChild(IDirectoryItem value)
        {
            var node = new TreeNode<IDirectoryItem>(value) { Parent = this };
            children.Add(node);
        }

        public bool RemoveChild(TreeNode<IDirectoryItem> node)
        {
            return children.Remove(node);
        }

        public List<TreeNode<IDirectoryItem>> Children()
        {
            return children;
        }

    }
}
