namespace Challenge5.Model
{
    public class Drive
    {
        public string Name { get; set; }
        public string Letter { get; set; }
        public long Size { get; set; }
        public long UsedSize { get; set; }
        public bool IsRemovable { get; set; }
        public bool IsValid { get; set; }

        public Drive(string name, string letter, long size, long usedSize, bool isRemovable = false, bool isValid = true)
        {
            if (string.IsNullOrEmpty(name))
                Name = "로컬 디스크";
            else
                Name = name;

            Letter = letter;
            Size = size;
            UsedSize = usedSize;
            IsRemovable = isRemovable;
            IsValid = isValid;
        }
    }
}