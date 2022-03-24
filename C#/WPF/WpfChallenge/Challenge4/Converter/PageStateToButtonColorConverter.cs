using Challenge4.State;

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Challenge4.Converter
{
    [ValueConversion(typeof(PageState), typeof(SolidColorBrush), ParameterType = typeof(PageState))]
    public class PageStateToButtonColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                throw new ArgumentNullException();

            if ((PageState)value == (PageState)parameter)
                return new SolidColorBrush(Colors.DeepSkyBlue);

            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
