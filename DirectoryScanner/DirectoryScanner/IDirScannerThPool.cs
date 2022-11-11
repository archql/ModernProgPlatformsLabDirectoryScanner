namespace lab3DirectoryScanner.DirectoryScanner
{
    internal interface IDirScannerThPool
    {
        bool Finished { get; }
        void Start(int maxThreadCount);
        void Shedule(DirScannerTask task);
        void Stop();
        void WaitForCompletion();
    }
}