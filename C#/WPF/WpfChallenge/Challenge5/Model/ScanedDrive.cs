using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge5.Model
{
    class ScanedDrive : NotifiableModel
    {
        private long totalScanedFileCount;
        public long TotalScanedFileCount
        {
            get => totalScanedFileCount;
            set
            {
                totalScanedFileCount = value;
                OnPropertyChanged();
            }
        }

        private long failedFileCount;
        public long FailedFileCount
        {
            get => failedFileCount;
            set
            {
                failedFileCount = value;
                OnPropertyChanged();
            }
        }


    }
}
