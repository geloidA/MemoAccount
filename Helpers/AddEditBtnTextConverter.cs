using System.Globalization;
using System.Windows.Data;

namespace MemoAccount.Helpers;

public class AddEditBtnTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is true ? "Изменить" : "Создать";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}