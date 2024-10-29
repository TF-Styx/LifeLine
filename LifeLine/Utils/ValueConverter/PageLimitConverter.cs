using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LifeLine.Utils.ValueConverter
{
    internal class PageLimitConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int currentPage && values[1] is int totalPage && values[2] is string buttonName)
            {
                if (buttonName == "Предыдущая")
                {
                    return currentPage > 1;
                }
                else if (buttonName == "Следующая")
                {
                    return currentPage < totalPage;
                }
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
