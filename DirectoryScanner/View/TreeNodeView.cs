using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media;

namespace lab3DirectoryScanner.View
{
    internal class TreeNodeView
    {
        public readonly DirTreeManager.TreeNode Node;

        public string Name
        {
            get
            {
                return Path.GetFileName(Node.Path);
            }
        }
        public long TotalSize
        {
            get
            {
                return Node.TotalSize;
            }
        }
        public double PercentSize
        {
            get
            {
                if (Node.Percent >= 0.0)
                    return Node.Percent;
                else
                    return 100.0;
            }
        }

        public string Image
        {
            get
            {
                return Path.GetFullPath(_imgs[Node.Type]);
            }
        }

        public TreeNodeView(DirTreeManager.TreeNode node)
        {
            Node = node;
            TreeViewNodes = new ObservableCollection<TreeNodeView>();
            foreach (var child in node.Childs)
            {
                TreeViewNodes.Add(new TreeNodeView(child));
            }
        }

        public ObservableCollection<TreeNodeView> TreeViewNodes { get; set; }

        private static Dictionary<DirTreeManager.TreeNodeType, string> _imgs = new()
        {
            { DirTreeManager.TreeNodeType.File, "resources/file.png" },
            { DirTreeManager.TreeNodeType.Directory, "resources/dir.png" }
        };
    }
}