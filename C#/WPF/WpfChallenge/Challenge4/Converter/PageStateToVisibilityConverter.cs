using Challenge4.State;

using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace Challenge4.Converter
{
    [ValueConversion(typeof(PageState), typeof(Visibility), ParameterType = typeof(PageState))]
    public class PageStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                throw new ArgumentNullException();

            if ((PageState)value == (PageState)parameter)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}