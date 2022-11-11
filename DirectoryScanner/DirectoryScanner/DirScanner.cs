using lab3DirectoryScanner.DirTreeManager;

namespace lab3DirectoryScanner.DirectoryScanner
{
    public class DirScanner
    {
        public readonly string _directoryPath;
        public int _threadCount;

        private IDirScannerThPool _dirScannerThPool;
        private ITreeManager _treeManager;

        public bool Finished => _dirScannerThPool.Finished;

        public DirScanner(string dir)
        {
            dir = Path.GetFullPath(dir);

            _directoryPath = dir;
            _dirScannerThPool = new DirScannerThPool();
            _treeManager = new TreeManager(new TreeNode(dir));
        }

        public void Start(int threadCount)
        {
            _threadCount = threadCount;
            if (!Directory.Exists(_directoryPath))
            {
                throw new DirectoryNotFoundException("Not found: " + _directoryPath);
            }
            _dirScannerThPool.Start(threadCount);
            _dirScannerThPool.Shedule(new DirScannerTask(_treeManager, _treeManager.Head()));
        }

        public void Stop()
        {
            _dirScannerThPool.Stop();
        }

        public void WaitForCompletion()
        {
            _dirScannerThPool.WaitForCompletion();
        }

        public ITreeManager Result()
        {
            return _treeManager;
        }
    }
}