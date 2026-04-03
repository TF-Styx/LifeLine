using Shared.WPF.Enums;
using System.Globalization;
using System.Windows.Data;

namespace Shared.WPF.Resources.ValueConverters
{
    public sealed class SaveStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SaveStatus status)
            {
                Func<string> func = status switch
                {
                    SaveStatus.Local => () => "Локальные данные",
                    SaveStatus.DataBase => () => "Сохраненные данные",

                    _ => () => "Не определено!"
                };

                return func.Invoke();
            }

            return "Не определено!";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
