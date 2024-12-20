using LifeLine.MVVM.Models.AppModel;
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
                if (employee != null)
                {
                    return $"{employee.SecondName} {employee.FirstName} {employee.LastName}";
                }
                else
                {
                    return "Сотрудник";
                }
            }
            if (value is Patient patient)
            {
                if (patient != null)
                {
                    return $"{patient.SecondName} {patient.FirstName} {patient.LastName}";
                }
                else
                {
                    return "Пациент";
                }
            }
            else if (value is EmployeeTimeTable timeTable)
            {
                return $"{timeTable.SecondName} {timeTable.FirstName} {timeTable.LastName}";
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
