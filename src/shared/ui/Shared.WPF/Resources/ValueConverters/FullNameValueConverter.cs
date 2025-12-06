using System.Globalization;
using System.Windows.Data;
using System.Xml.Linq;

namespace Shared.WPF.Resources.ValueConverters
{
    public sealed class FullNameValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var prefix = string.Empty;

            if (parameter != null && parameter is string value)
                prefix = value;

            if (values[0] is string surname && values[1] is string name)
            {
                if (values[2] is string patronymic)
                    return BuildResult(surname, name, patronymic, prefix);

                return BuildResult(surname, name, null, prefix);
            }

            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string BuildResult(string surname, string name, string? patronymic, string? prefix)
        {
            if (!string.IsNullOrWhiteSpace(patronymic) && !string.IsNullOrWhiteSpace(prefix))
                return $"{prefix}{surname} {name} {patronymic}";

            if (string.IsNullOrWhiteSpace(patronymic) && string.IsNullOrWhiteSpace(prefix))
                return $"{surname} {name}";

            if (!string.IsNullOrWhiteSpace(patronymic) && string.IsNullOrWhiteSpace(prefix))
                return $"{surname} {name} {patronymic}";

            if (string.IsNullOrWhiteSpace(patronymic) && !string.IsNullOrWhiteSpace(prefix))
                return $"{prefix}{surname} {name}";

            return string.Empty;
        }
    }
}