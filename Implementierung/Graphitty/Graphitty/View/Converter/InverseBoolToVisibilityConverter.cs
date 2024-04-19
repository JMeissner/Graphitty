using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Graphitty.View.Converter
{
    /// <summary>
    /// Used to convert false to visible or true to hidden
    /// </summary>
    internal class InverseBoolToVisibilityConverter : IValueConverter
    {
        #region Private Fields

        private BooleanToVisibilityConverter _converter = new BooleanToVisibilityConverter();

        #endregion Private Fields

        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var result = _converter.Convert(value, targetType, parameter, culture) as Visibility?;
            return result == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var result = _converter.ConvertBack(value, targetType, parameter, culture) as bool?;
            return result == true ? false : true;
        }

        #endregion Public Methods
    }
}