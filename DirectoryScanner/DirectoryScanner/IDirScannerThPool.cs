namespace lab3DirectoryScanner.DirectoryScanner
{
    internal interface IDirScannerThPool
    {
        bool Finished { get; }
        void Start(int maxThreadCount);
        void Sheudule(DirScannerTask task);
        void Stop();
    }
}