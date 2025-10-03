using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LifeLine.Utils.ValueConverter
{
    class GraphDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                if (date != DateTime.MinValue)
                {
                    string formattedDate = date.ToString("dddd, d MMMM yyyy г.", new CultureInfo("ru-RU"));
                    return char.ToUpper(formattedDate[0]) + formattedDate.Substring(1);
                }
            }
            return "Дата";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
