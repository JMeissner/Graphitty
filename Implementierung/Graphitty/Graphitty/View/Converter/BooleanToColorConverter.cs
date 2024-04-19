using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Graphitty.View.Converter
{
    /// <summary>
    /// Converts a boolean value to a color. If true then green will be returned. Otherwise red will be returned.
    /// </summary>
    class BooleanToColorConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool && (bool)value)
            {
                return new SolidColorBrush(Colors.Green);
            }
            return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}