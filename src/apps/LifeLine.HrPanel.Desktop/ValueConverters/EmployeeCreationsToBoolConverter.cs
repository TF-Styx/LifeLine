using LifeLine.HrPanel.Desktop.Enums;
using System.Globalization;
using System.Windows.Data;

namespace LifeLine.HrPanel.Desktop.ValueConverters
{
    internal class EmployeeCreationsToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EmployeeCreationSteps valueSteps && parameter is EmployeeCreationSteps parameterSteps)
            {
                if (valueSteps == parameterSteps)
                    return true;

                return false;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
