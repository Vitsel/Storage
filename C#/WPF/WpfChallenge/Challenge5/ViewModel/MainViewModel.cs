using Challenge5.State;
using Challenge5.Model;
using System.Collections.Generic;
using System.IO;

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

        public Drive SelectedDrive { get; set; }
        public List<Drive> DriveList { get; set; }

        public MainViewModel()
        {
            DriveList = GetDriveList();
            PageState = PageState.DriveList;
        }

        public void SetStatus(PageState state)
        {
            PageState = state;
        }

        public void ScanDrive()
        {

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
                    var size = 0l;
                    var usedSize = 0l;
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