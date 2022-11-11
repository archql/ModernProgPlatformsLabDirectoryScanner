using lab3DirectoryScanner.DirTreeManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3DirectoryScanner.DirectoryScanner
{
    internal class DirScannerTask
    {
        private ITreeManager _treeManager;
        private TreeNode _parent;

        public DirScannerTask (ITreeManager treeManager, TreeNode parent)
        {
            _treeManager = treeManager;
            _parent = parent;
        }

        public void run(ConcurrentQueue<DirScannerTask> taskQueue)
        {
            string dir = _parent.Path;
            // list all in the directory (nonrecursively)
            // 
            FileAttributes attr = File.GetAttributes(dir);
            // supposed to be an existent directory
            if (!attr.HasFlag(FileAttributes.Directory) && !Directory.Exists(dir))
            {
                return;
            }
            // loop through all entries in it
            var entries = Directory.GetFileSystemEntries(dir);
            foreach (var subDir in entries)
            {
                FileAttributes subAttr = File.GetAttributes(subDir);
                FileInfo subInfo = new FileInfo(subDir);
                if (!subInfo.Exists)
                {
                    continue;
                }
                if (subAttr.HasFlag(FileAttributes.ReparsePoint)) // not a symbolic link
                {
                    continue;
                }

                TreeNode node = new TreeNode(subDir);
                if (subAttr.HasFlag(FileAttributes.Directory))
                {
                    // queue new task
                    node.Type = TreeNodeType.Directory;
                    taskQueue.Enqueue(new DirScannerTask(_treeManager, node));
                }
                else
                {
                    // get file size
                    node.Size = subInfo.Length;
                    node.Type = TreeNodeType.File;
                }
                // add element to the tree
                _treeManager.AddChildTo(_parent, node);
            }

        }
    }
}
