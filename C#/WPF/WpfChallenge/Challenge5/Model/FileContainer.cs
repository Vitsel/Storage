using Challenge5.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Challenge5.Model
{
    public class FileContainer : NotifiableModel
    {
        public string Extension { get; set; }
        public List<FileItem> Files { get; set; }
        public long Count { get => Files.Count; }
        public long TotalSize { get; set; }

        public FileContainer(string extension)
        {
            Extension = extension;
            Files = new List<FileItem>();
        }

        public void Add(string path)
        {
            Files.Add(new FileItem(path));
            TotalSize = Files.Sum(f => f.Size);
        }

        public override string ToString()
        {
            return $"{Extension} | {Count}";
        }
    }
}
