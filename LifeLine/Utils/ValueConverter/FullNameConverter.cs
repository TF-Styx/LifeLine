using LifeLine.MVVM.Models.MSSQL_DB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LifeLine.Utils.ValueConverter
{
    class FullNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Employee employee)
            {
                return $"{employee.SecondName} {employee.FirstName} {employee.LastName}";
            }
            else
            {
                return "Сотрудник";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
