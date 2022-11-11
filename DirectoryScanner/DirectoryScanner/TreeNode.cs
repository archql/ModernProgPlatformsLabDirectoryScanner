using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3DirectoryScanner.DirTreeManager
{
    public class TreeNode
    {
        public readonly List<TreeNode>Childs;

        public string Path { get; private set; }
        public TreeNodeType Type { get; set; } = TreeNodeType.Directory;
        public long Size { get; set; } = -1;

        public double Percent { get; private set; } = -1.0;
        public long TotalSize { get; private set; } = -1;


        public TreeNode (string path)
        {
            Childs = new List<TreeNode>();
            Path = path;
        }

        internal void AddChild(TreeNode child)
        {
            Childs.Add(child);
        }

        internal long CalcSize()
        {
            long totalSize = 0;
            if (Type != TreeNodeType.Directory)
                totalSize = Size;
            // calc total size
            foreach (TreeNode child in Childs)
            {
                totalSize += child.CalcSize();
            }
            TotalSize = totalSize;

            // set percentages
            foreach (TreeNode child in Childs)
            {
                child.Percent = (double)child.TotalSize / totalSize * 100.0;
            }

            return totalSize;
        }

        public void PrintToConsole(int level = 0)
        {
            string name = Type == TreeNodeType.Directory ? "dir" : "file";

            for (int i = 0; i < level; i++)
                Console.Out.Write('\t');
            Console.Out.WriteLine(name + " " + Path + " " + TotalSize + " (" + string.Format("{0:0.##}", Percent) + "%)");

            foreach (TreeNode child in Childs)
            {
                child.PrintToConsole(level + 1);
            }
        }
    }

    public enum TreeNodeType
    {
        File,
        Directory
    }
}
