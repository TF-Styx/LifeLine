using LifeLine.HrPanel.Desktop.Enums;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LifeLine.HrPanel.Desktop.ValueConverters
{
    internal sealed class TypeActionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TypeAction typeAction && parameter is TypeAction param)
            {
                if (typeAction == param)
                    return Visibility.Visible;

                return Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
