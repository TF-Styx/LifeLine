using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace LifeLine.Utils.ValueConverter
{
    class SecondNameToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string FirstLetter)
            {
                if (FirstLetter.StartsWith("С"))
                {
                    return (Brush)new BrushConverter().ConvertFromString("Red");
                }
                else
                {
                    return (Brush)new BrushConverter().ConvertFromString("Black");
                }
            }
            else
            {
                return (Brush)new BrushConverter().ConvertFromString("Black");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
