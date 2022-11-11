using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3DirectoryScanner.DirectoryScanner
{
    internal class DirScannerThPool : IDirScannerThPool
    {
        Semaphore _semaphore;

        private readonly ConcurrentQueue<DirScannerTask> _taskQueue;
        private readonly CancellationTokenSource _cancellationTokenSrc;

        private bool _isScanning;
        private int _maxThreadCount, _threadCount;

        Thread _scannerThread;

        public bool Finished => (_taskQueue.IsEmpty && _threadCount == _maxThreadCount) || _cancellationTokenSrc.IsCancellationRequested;

        public DirScannerThPool()
        {
            _taskQueue = new ConcurrentQueue<DirScannerTask>();
            _cancellationTokenSrc = new CancellationTokenSource();
        }
        public void Start(int maxThreadCount)
        {
            _threadCount = _maxThreadCount = maxThreadCount;
            _semaphore = new Semaphore(maxThreadCount, maxThreadCount);
            _isScanning = true;

            _scannerThread = new Thread(ScannerWorkout);
            _scannerThread.Start(_cancellationTokenSrc.Token);
        }

        public void Stop()
        {
            _cancellationTokenSrc.Cancel();
        }

        public void Shedule(DirScannerTask task)
        {
            _taskQueue.Enqueue(task);
        }

        private void ScannerWorkout(object ?param)
        {
            if (param == null)
                return;
            CancellationToken token = (CancellationToken)param;

            while (!Finished)
            {
                if (_taskQueue.IsEmpty)
                    continue;
                _semaphore.WaitOne();

                if (_taskQueue.TryDequeue(out var task))
                {
                    Interlocked.Decrement(ref _threadCount);
                    ThreadPool.QueueUserWorkItem(TaskWorkout, task, true);
                }
            }
        }

        private void TaskWorkout(object? param)
        {
            try
            {
                if (param != null)
                {
                    var task = (DirScannerTask)param;
                    task.run(_taskQueue);
                }
            }
            catch { }
            finally
            {
                Interlocked.Increment(ref _threadCount);
                _semaphore.Release();
            }
        }

        public void WaitForCompletion()
        {
            _scannerThread.Join();
        }
    }
}
