using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using lab3DirectoryScanner.DirectoryScanner;
using System.Collections.ObjectModel;

namespace lab3DirectoryScanner.View
{
    internal class DirScannerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        DirScanner? _dirScanner;

        private string? _filePath;
        private bool _isFileChosen = false;
        private bool _isSearching = false;
        private int _threadCount = 5;

        private ObservableCollection<TreeNodeView> _treeNodes;

        private RelayCommand? _stopSearch;
        private RelayCommand? _runSearch;
        private RelayCommand? _chooseFile;

        public string FilePath
        {
            get
            {
                return _filePath ?? "(not chosen)";
            }
            set
            {
                _filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        public RelayCommand ChooseFile
        {
            get
            {
                return _chooseFile ??= new RelayCommand(obj =>
                {
                    using var dialog = new FolderBrowserDialog();

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        FilePath = dialog.SelectedPath;
                        IsFileChosen = true;
                    }
                });
            }
        }

        public RelayCommand RunSearch
        {
            get
            {
                return _runSearch ??= new RelayCommand(obj =>
                {
                    if (_filePath == null)
                        return;
                    _dirScanner = new DirScanner(_filePath);

                    Task.Run(() =>
                    {
                        IsSearching = true;
                        _dirScanner.Start(_threadCount);

                        _dirScanner.WaitForCompletion();

                        var result = _dirScanner.Result();
                        result.CalcSize();

                        IsSearching = false;

                        TreeResult = result;
                    });
                });
            }
        }

        public RelayCommand StopSearch
        {
            get
            {
                return _stopSearch ??= new RelayCommand(obj =>
                {
                    if (_dirScanner == null)
                        return;
                    _dirScanner.Stop();

                    IsSearching = false;
                });
            }
        }

        public DirTreeManager.ITreeManager TreeResult
        {
            set
            {
                if (value != null)
                {
                    _treeNodes = new ObservableCollection<TreeNodeView>();
                    _treeNodes.Add(new TreeNodeView(value.Head()));
                    OnPropertyChanged("TreeViewList");
                }
            }
        }
        public ObservableCollection<TreeNodeView> TreeViewList
        {
            get
            {
                return _treeNodes;
            }
            set
            {
                _treeNodes = value;
            }
        }

        public bool IsFileChosen
        {
            get
            {
                return _isFileChosen;
            }
            set
            {
                _isFileChosen = value;
                OnPropertyChanged("IsFileChosen");
                OnPropertyChanged("IsReady");
            }
        }

        public bool IsSearching
        {
            get
            {
                return _isSearching;
            }
            set
            {
                _isSearching = value;
                OnPropertyChanged("IsSearching");
                OnPropertyChanged("IsReady");
            }
        }

        public bool IsReady
        {
            get
            {
                return !_isSearching && _isFileChosen;
            }
        }

        public int ThreadCount
        {
            get
            {
                return _threadCount;
            }
            set
            {
                if (value > 2)
                {
                    _threadCount = value;
                    OnPropertyChanged("ThreadCount");
                }
            }
        }

        public void OnPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
