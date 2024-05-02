using System.Globalization;
using System.Windows.Data;
using MemoAccount.Models;

namespace MemoAccount.Helpers;

public class MemoToStatusChangeString : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is Memo { Status: MemoStatus.Open } ? "Отметить выполенным" : "Отметить невыполненным";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}