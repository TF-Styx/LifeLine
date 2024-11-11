using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LifeLine.Utils.ValueConverter
{
    class TimeOnlyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value is string time)
            //{
            //    return time;
            //}
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("HH:mm"); // Для 24-часового формата
                                                   // return dateTime.ToString("hh:mm tt"); // Для 12-часового формата
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
