using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Project_Nirvash.Converters
{
    /// <summary>
    /// IntToVisibilityConverter class
    /// </summary>
    public class IntToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Convert int or Nullable int to Visibility.
        /// </summary>
        /// <param name="value">int or Nullable int</param>
        /// <param name="targetType">Visibility</param>
        /// <param name="parameter">null</param>
        /// <param name="language">null</param>
        /// <returns>Visible or Collapsed</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (int?)value == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Interface method not implemented.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
