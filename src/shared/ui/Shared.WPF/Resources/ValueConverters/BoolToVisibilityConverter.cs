using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Shared.WPF.Resources.ValueConverters
{
    public sealed class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool state)
                if (state)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
             
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
