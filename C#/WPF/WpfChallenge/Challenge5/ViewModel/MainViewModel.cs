using Challenge5.State;
using Challenge5.Model;
using Challenge5.Control;

using System.IO;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using Challenge5.Command;
using System;
using System.Windows.Threading;

namespace Challenge5.ViewModel
{
    public class MainViewModel : NotifiableModel
    {

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

        public Dictionary<string, Type> FailLogs { get; set; }
        public long FailedFileCount { get => FailLogs.Count; }
        public long TotalScanedFileCount { get; set; }
        public long MostCount { get; set; }
        public List<Drive> DriveList { get; set; }
        public Drive SelectedDrive { get; set; }
        public ObservableCollection<FileContainer> FileContainers { get; set; }
        public ICommand CancelCommand { get; set; }
        public bool IsLimitUiUpdateRate { get; set; }

        private CancellationTokenSource cancelToken;
        private Task ScanTask;

        public MainViewModel()
        {
            DriveList = GetDriveList();
            PageState = PageState.DriveList;
            CancelCommand = new RelayCommand(() => cancelToken?.Cancel());

            InitScanData();
        }

        public void SetPage(PageState state)
        {
            PageState = state;
        }

        public async void ScanDriveAsync()
        {
            if (SelectedDrive == null)
                return;

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(IsLimitUiUpdateRate ? 250 : 1);
            timer.Tick += NotifyScanProperty;
            cancelToken = new CancellationTokenSource();

            ScanTask = Task.Run(() => ScanAllFiles(SelectedDrive.Letter, cancelToken.Token), cancelToken.Token);
            timer.Start();
            await ScanTask;
            NotifyScanProperty(null, null);
            timer.Stop();

            MessageBox.Show(string.Format("스캔이 {0}되었습니다.", cancelToken.IsCancellationRequested ? "중지" : "완료"));

            ScanTask = null;
            cancelToken = null;
        }

        private void NotifyScanProperty(object sender, EventArgs e)
        {
            OnPropertyChanged("MostCount");
            OnPropertyChanged("totalScanedFileCount");
            OnPropertyChanged("FileContainers");
            OnPropertyChanged("FailedFileCount");
        }

        private void InitScanData()
        {
            SelectedDrive = null;
            FileContainers = new ObservableCollection<FileContainer>();
            TotalScanedFileCount = MostCount = 0;
            FailLogs = new Dictionary<string, Type>();
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

                    container.Add(file.FullName);

                    TotalScanedFileCount = FileContainers.Sum(f => f.Count);
                    MostCount = FileContainers.Max(f => f.Count);
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