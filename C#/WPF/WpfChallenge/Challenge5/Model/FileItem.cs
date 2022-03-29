using System.IO;
using System.Drawing;

namespace Challenge5.Model
{
    public class FileItem
    {
        public string Name { get; private set; }
        public long Size { get; private set; }
        public bool IsImage { get; private set; }
        public Image Thumbnail { get; private set; }

        public FileItem(string path)
        {
            Name = path;
            var fi = new FileInfo(path);
            Size = fi.Length;
            IsImage = IsImageExtension(fi.Extension);
        }

        private bool IsImageExtension(string extension)
        {
            var ext = extension.ToLower();

            if (ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".gif" || ext == ".bmp")
                return true;

            return false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
