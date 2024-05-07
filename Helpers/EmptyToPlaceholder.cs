using System.Globalization;
using System.Windows.Data;

namespace MemoAccount.Helpers;

public class EmptyToPlaceholder : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null || value is string s && string.IsNullOrWhiteSpace(s) ? parameter : value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}