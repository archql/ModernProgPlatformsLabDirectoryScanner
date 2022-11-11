using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3DirectoryScanner.DirTreeManager
{
    public class TreeNode
    {
        List<TreeNode> _childs;

        public string Path { get; private set; }
        public TreeNodeType Type { get; set; } = TreeNodeType.Directory;
        public long Size { get; set; } = -1;

        public double Percent { get; private set; } = -1.0;
        public long TotalSize { get; private set; } = -1;


        public TreeNode (string path)
        {
            _childs = new List<TreeNode>();
            Path = path;
        }

        internal void AddChild(TreeNode child)
        {
            _childs.Add(child);
        }

        internal long GetSize()
        {
            long totalSize = 0;
            if (Type != TreeNodeType.Directory)
                totalSize = Size;
            // calc total size
            foreach (TreeNode child in _childs)
            {
                totalSize += child.GetSize();
            }
            // set percentages
            foreach (TreeNode child in _childs)
            {
                Percent = child.TotalSize / totalSize * 100;
            }

            TotalSize = totalSize;
            return totalSize;
        }
    }

    public enum TreeNodeType
    {
        File,
        Directory
    }
}
