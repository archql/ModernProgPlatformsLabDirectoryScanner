using lab3DirectoryScanner.DirTreeManager;

namespace lab3DirectoryScanner.DirectoryScanner
{
    public class DirScanner
    {
        public readonly string _directoryPath;
        public int _threadCount;

        private IDirScannerThPool _dirScannerThPool;
        private ITreeManager _treeManager;

        public DirScanner(string dir)
        {
            _directoryPath = dir;
            _dirScannerThPool = new DirScannerThPool();
            _treeManager = new TreeManager(new TreeNode(dir));
        }

        public void Start(int threadCount)
        {
            _threadCount = threadCount;
            var fileInfo = new FileInfo(_directoryPath);

            if (!fileInfo.Exists)
            {
                throw new DirectoryNotFoundException("Not found: " + _directoryPath);
            }
            FileAttributes attr = File.GetAttributes(_directoryPath);
            if (!attr.HasFlag(FileAttributes.Directory))
            {
                throw new ArgumentException("Supposed to be a directory path: " + _directoryPath);
            }
            _dirScannerThPool.Start(threadCount);
            _dirScannerThPool.Shedule(new DirScannerTask(_treeManager, _treeManager.Head()));
        }

        public void Stop()
        {
            _dirScannerThPool.Stop();
        }

        public bool result()
        {
            return false;
        }
    }
}