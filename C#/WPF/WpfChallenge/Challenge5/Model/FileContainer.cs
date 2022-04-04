using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Challenge5.Model
{
    public class FileContainer : NotifiableModel
    {
        public string Extension { get; set; }
        public ObservableCollection<FileItem> Files { get; set; }
        public long Count { get => Files.Count; }
        public long TotalSize { get; set; }

        public FileContainer(string extension)
        {
            Extension = extension;
            Files = new ObservableCollection<FileItem>();
        }

        public void AddRange(IEnumerable<FileItem> second)
        {
            foreach(var f in second)
                Files.Add(f);

            TotalSize = Files.Sum(f => f.Size);
        }

        public override string ToString()
        {
            return $"{Extension} | {Count}";
        }
    }
}