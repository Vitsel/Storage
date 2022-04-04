using Challenge5.Command;
using Challenge5.Model;
using Challenge5.State;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Challenge5.ViewModel
{
    public class MainViewModel : NotifiableModel
    {
        public event EventHandler PropUpdate;

        private PageState pageState;
        public PageState PageState
        {
            get => pageState;
            set
            {
                pageState = value;
                OnPropertyChanged();
            }
        }

        public FileContainer SelectedFileContainer { get; set; }
        public long TotalScanedFileCount { get; set; }
        public long MostCount { get; set; }
        public Dictionary<string, Type> FailLogs { get; set; }
        public long FailedFileCount { get => FailLogs.Count; }
        public List<Drive> DriveList { get; set; }
        public Drive SelectedDrive { get; set; }
        public ICommand CancelCommand { get; set; }
        public bool IsLimitUiUpdateRate { get; set; }
        public ObservableCollection<FileContainer> FileContainers { get; set; }
        public ObservableCollection<System.Drawing.Image> Thumbnails { get; set; }

        private List<Tuple<FileContainer, List<FileItem>>> NewFileBuffer;
        private CancellationTokenSource cancelToken;

        public MainViewModel()
        {
            DriveList = GetDriveList();
            PageState = PageState.DriveList;
            CancelCommand = new RelayCommand(() => cancelToken?.Cancel());
            IsLimitUiUpdateRate = true;

            InitScanData();
        }

        private void InitScanData()
        {
            SelectedDrive = null;
            FileContainers = new ObservableCollection<FileContainer>();
            TotalScanedFileCount = MostCount = 0;
            FailLogs = new Dictionary<string, Type>();
            NewFileBuffer = new List<Tuple<FileContainer, List<FileItem>>>();
            Thumbnails = new ObservableCollection<System.Drawing.Image>();
        }

        public void SelectFileContainer(FileContainer container)
        {
            SelectedFileContainer = container;
            OnPropertyChanged("SelectedFileContainer");
        }

        public void SetPage(PageState state)
        {
            PageState = state;
        }

        public async void ScanDriveAsync()
        {
            var obsrv = Observable.FromEventPattern(this, "PropUpdate")
                .Sample(TimeSpan.FromMilliseconds(IsLimitUiUpdateRate ? 250 : 0))
                .Subscribe(NotifyScanProperty);

            if (SelectedDrive == null)
                return;

            cancelToken = new CancellationTokenSource();

            await Task.Run(() => ScanAllFiles(SelectedDrive.Letter, cancelToken.Token), cancelToken.Token);

            MessageBox.Show(string.Format("스캔이 {0}되었습니다.", cancelToken.IsCancellationRequested ? "중지" : "완료"));

            cancelToken = null;
        }

        public void OpenDirectory(string path)
        {
            try
            {
                Process.Start(path);
            }
            catch
            {
                MessageBox.Show("폴더가 존재하지 않습니다.");
            }
        }

        private void NotifyScanProperty(object sender)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                AddFileToContainer();
                TotalScanedFileCount = FileContainers.Sum(f => f.Count);
                MostCount = FileContainers.Max(f => f.Count);
            });

            OnPropertyChanged("MostCount");
            OnPropertyChanged("totalScanedFileCount");
            OnPropertyChanged("FileContainers");
            OnPropertyChanged("FailedFileCount");
            OnPropertyChanged("Thumbnails");
        }

        private void AddFileToContainer()
        {
            NewFileBuffer.ForEach(n =>
            {
                n.Item1.AddRange(n.Item2);
                n.Item2.ForEach(f =>
                {
                    if (f.IsImage)
                        Thumbnails.Insert(0, f.Thumbnail);
                    if (Thumbnails.Count > 10)
                        Thumbnails.RemoveAt(10);
                });
            });

            NewFileBuffer.Clear();
        }

        private void AddToBuffer(FileContainer container, string path)
        {
            try
            {
                var added = NewFileBuffer.SingleOrDefault(c => c.Item1.Extension == container.Extension);
                if (added != null)
                {
                    added.Item2.Add(new FileItem(path));
                    return;
                }

                var newContainer = new Tuple<FileContainer, List<FileItem>>(container, new List<FileItem>());
                newContainer.Item2.Add(new FileItem(path));
                NewFileBuffer.Add(newContainer);
            }
            catch (Exception ex)
            {
                FailLogs.Add(ex.Message, ex.GetType());
            }

        }

        private void ScanAllFiles(string path, CancellationToken token)
        {
            if (!File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                return;

            try
            {
                var parent = new DirectoryInfo(path + "\\");
                var files = parent.GetFiles();

                foreach (var file in files)
                {
                    token.ThrowIfCancellationRequested();

                    var ext = string.IsNullOrEmpty(file.Extension) ? "(N/A)" : file.Extension.ToLower();
                    var container = FileContainers.SingleOrDefault(f => f.Extension == ext);

                    if (container == null)
                    {
                        container = new FileContainer(ext);
                        Application.Current.Dispatcher.Invoke(() => FileContainers.Add(container));
                    }

                    Application.Current.Dispatcher.Invoke(() => AddToBuffer(container, file.FullName));

                    PropUpdate(null, null);
                }

                foreach (var dir in parent.GetDirectories())
                {
                    token.ThrowIfCancellationRequested();

                    ScanAllFiles(dir.FullName, token);
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                FailLogs.Add(ex.Message, ex.GetType());
                PropUpdate(null, null);
                return;
            }
        }

        private List<Drive> GetDriveList()
        {
            var driveList = new List<Drive>();

            try
            {
                foreach (var di in DriveInfo.GetDrives())
                {
                    var isValid = true;
                    var letter = di.Name.TrimEnd('\\');
                    var name = "";
                    var size = 0L;
                    var usedSize = 0L;
                    var isRemovable = di.DriveType == DriveType.Removable;

                    try
                    {
                        name = di.VolumeLabel;
                    }
                    catch
                    {
                        isValid = false;
                    }

                    try
                    {
                        size = di.TotalSize;
                    }
                    catch
                    {
                        isValid = false;
                    }

                    try
                    {
                        usedSize = di.TotalSize - di.TotalFreeSpace;
                    }
                    catch
                    {
                        isValid = false;
                    }

                    driveList.Add(new Drive(name, letter, size, usedSize, isRemovable, isValid));
                }
            }
            catch
            {
                return null;
            }

            return driveList;
        }
    }
}