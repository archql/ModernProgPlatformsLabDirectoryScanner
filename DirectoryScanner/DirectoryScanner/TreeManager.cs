using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3DirectoryScanner.DirTreeManager
{
    public class TreeManager : ITreeManager
    {
        private object _locker;
        private readonly TreeNode _treeHead;

        public TreeManager(TreeNode head)
        {
            _locker = new object();
            _treeHead = head;
        }

        public void AddChildTo(TreeNode parent, TreeNode child)
        {
            lock(_locker)
            {
                parent.AddChild(child);
            }
        }
        public TreeNode Head()
        {
            return _treeHead;
        }


    }
}
