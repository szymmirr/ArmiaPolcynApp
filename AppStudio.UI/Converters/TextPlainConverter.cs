using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace AppStudio.Controls
{
    public class TextPlainConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string plainText = String.Empty;
            try
            {
                if (value != null)
                {
                    string text = value.ToString();
                    if (text.Length > 0)
                    {
                        plainText = HtmlUtil.CleanHtml(text);
                        if (parameter != null)
                        {
                            int maxLength = 0;
                            Int32.TryParse(parameter.ToString(), out maxLength);
                            if (maxLength > 0)
                            {
                                plainText = HtmlUtil.Truncate(plainText, maxLength, "...");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogs.WriteError("TextPlainConverter.Convert", ex);
            }
            return plainText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
