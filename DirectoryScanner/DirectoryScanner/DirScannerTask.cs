using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3DirectoryScanner.DirectoryScanner
{
    internal class DirScannerTask
    {
        private DirScannerTask ?_parent;
        private string _dir; 

        public DirScannerTask (string dir)
        {
            _parent = null;
            _dir = dir;
        }

        public void run()
        {
            // list all in the directory (nonrecursively)
            // 
        }
    }
}
