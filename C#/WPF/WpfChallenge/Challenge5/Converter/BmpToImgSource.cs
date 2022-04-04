using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Challenge5.Converter
{
    class BmpToImgSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Bitmap bmp))
                return null;

            var img = new BitmapImage();
            var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Bmp);
            img.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            img.StreamSource = ms;
            img.EndInit();

            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
