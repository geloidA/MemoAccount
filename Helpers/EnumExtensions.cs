using System;
using System.ComponentModel;

public static class EnumExtensions
{
    public static string GetDescription<T>(this T value) where T : struct, Enum
    {
        if (!typeof(T).IsEnum)
        {
            throw new ArgumentException($"{value} must be an enum");
        }

        var fieldInfo = value.GetType().GetField(value.ToString());
        var attributes = (DescriptionAttribute[])fieldInfo!.GetCustomAttributes(typeof(DescriptionAttribute), false);

        return attributes.Length > 0 ? attributes[0].Description : value.ToString();
    }
}