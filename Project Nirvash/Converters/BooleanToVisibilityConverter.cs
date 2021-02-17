using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Project_Nirvash.Converters
{
    /// <summary>
    /// BooleanToVisibilityConverter class
    /// </summary>
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Convert bool or Nullable bool to Visibility.
        /// </summary>
        /// <param name="value">bool or Nullable bool</param>
        /// <param name="targetType">Visibility</param>
        /// <param name="parameter">null</param>
        /// <param name="language">null</param>
        /// <returns>Visible or Collapsed</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool bValue = false;
            if (value is bool boolean)
            {
                bValue = boolean;
            }
            else if (value is bool?)
            {
                bool? tmp = (bool?)value;
                bValue = tmp ?? false;
            }
            return bValue ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Convert Visibility to boolean.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            else
            {
                return false;
            }
        }
    }
}
