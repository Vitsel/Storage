using Challenge5.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Challenge5.Converter
{
    public class ItemsOrderLimit : IValueConverter
    {
        public object Convert(object items, Type targetType, object limit, CultureInfo culture)
        {
            int count;
            if (!int.TryParse((string)limit, out count))
                throw new ArgumentException();

            var t = ((IEnumerable<object>)items).OrderByDescending(i => ((FileContainer)i).Count);

            return ((IEnumerable<object>)items).OrderByDescending(i => ((FileContainer)i).Count).Take(count);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}