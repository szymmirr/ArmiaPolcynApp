using System;
using System.Globalization;
using System.Windows.Data;

namespace AppStudio.Controls
{
    public class MultilineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value as string;
            if (text == null) return string.Empty;
            text = text.Replace("\n", "<br>");
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}
