using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Challenge5.Converter
{
    class BytesToFormat : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double size;
            try
            {
                size = (long)value;
            }
            catch
            {
                throw new ArgumentException();
            }
            
            var units = new string[] { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            var i = 0;

            while(size >= 1024 && i < units.Length - 1)
            {
                i++;
                size = size / 1024;
            }

            return string.Format("{0:0}{1}", size, units[i]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
