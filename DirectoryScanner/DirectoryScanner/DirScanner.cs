namespace lab3DirectoryScanner.DirectoryScanner
{
    public class DirScanner
    {
        public readonly string _directoryPath;
        public int _threadCount;

        private IDirScannerThPool _dirScannerThPool;

        public DirScanner(string dir)
        {
            _directoryPath = dir;
            _dirScannerThPool = new DirScannerThPool();
        }

        public void Start(int threadCount)
        {
            _threadCount = threadCount;
            var fileInfo = new FileInfo(_directoryPath);

            if (fileInfo.Exists)
            {
                _dirScannerThPool.Start(threadCount);
                _dirScannerThPool.Sheudule(new DirScannerTask(_directoryPath));
            }
            else
            {
                throw new DirectoryNotFoundException("Not found: " + _directoryPath);
            }
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