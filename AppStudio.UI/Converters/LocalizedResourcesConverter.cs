using System;
using System.Windows.Data;

using AppStudio.Resources;

namespace AppStudio.Converters
{
    /// <summary>
    /// Returns a resource based on the key.
    /// </summary>
    public sealed class LocalizedResourcesConverter : IValueConverter
    {
        /// <summary>
        /// From a key returns a value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            var resourceName = value.ToString();
            return AppResources.ResourceManager.GetString(resourceName, culture);
        }

        /// <summary>
        /// Not implemented conversion to original value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
